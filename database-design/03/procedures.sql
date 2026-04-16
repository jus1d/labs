USE warehouse;

-- Архивная таблица для задания 6 (создаётся до хранимых процедур)
CREATE TABLE IF NOT EXISTS storage_units_archive (
    storage_id       INT PRIMARY KEY AUTO_INCREMENT,
    order_number     VARCHAR(50)    NOT NULL,
    order_date       DATE           NOT NULL,
    supplier_id      INT            NOT NULL,
    balance_account  VARCHAR(50),
    document_type_id INT,
    document_number  VARCHAR(50),
    material_id      INT            NOT NULL,
    material_account VARCHAR(50),
    unit_id          INT            NOT NULL,
    quantity         DECIMAL(15,3)  NOT NULL,
    price            DECIMAL(15,2)  NOT NULL
);

DELIMITER $$











-- peek1
-- 1. Хранимая процедура, выполняющая арифметическую операцию над полями таблицы
--    по вводимому параметру: вычисляет итоговую стоимость (количество * цена)
--    для каждой единицы хранения указанного поставщика.
CREATE PROCEDURE get_supplier_total_cost(IN p_supplier_id INT)
BEGIN
    SELECT
        su.storage_id,
        su.order_number,
        su.order_date,
        s.supplier_name,
        m.material_name,
        su.quantity,
        su.price,
        su.quantity * su.price AS total_cost
    FROM storage_units su
    JOIN suppliers s  ON su.supplier_id  = s.supplier_id
    JOIN materials m  ON su.material_id  = m.material_id
    WHERE su.supplier_id = p_supplier_id;
END$$

-- 2. Хранимая процедура, возвращающая связанные записи нескольких таблиц.
--    Выводит полную информацию об единицах хранения с данными поставщика,
--    материала и сопроводительного документа.
CREATE PROCEDURE get_storage_units_full(IN p_supplier_id INT)
BEGIN
    SELECT
        su.storage_id,
        su.order_number,
        su.order_date,
        s.supplier_name,
        s.inn,
        m.material_name,
        dt.document_name,
        su.document_number,
        u.unit_name,
        su.quantity,
        su.price
    FROM storage_units su
    JOIN      suppliers      s  ON su.supplier_id      = s.supplier_id
    JOIN      materials      m  ON su.material_id      = m.material_id
    LEFT JOIN document_types dt ON su.document_type_id = dt.document_type_id
    JOIN      units          u  ON su.unit_id          = u.unit_id
    WHERE su.supplier_id = p_supplier_id;
END$$

-- 3. Хранимая процедура, вычисляющая агрегированные характеристики записей таблицы.
--    Возвращает минимальную, максимальную и среднюю цену, а также суммарное
--    количество поступлений для указанного материала.
CREATE PROCEDURE get_material_price_stats(IN p_material_id INT)
BEGIN
    SELECT
        m.material_name,
        MIN(su.price)    AS min_price,
        MAX(su.price)    AS max_price,
        AVG(su.price)    AS avg_price,
        SUM(su.quantity) AS total_quantity
    FROM storage_units su
    JOIN materials m ON su.material_id = m.material_id
    WHERE su.material_id = p_material_id
    GROUP BY m.material_name;
END$$

-- 4. Функция с конструкцией CASE.
--    Преобразует номер дня недели (1 = Понедельник … 7 = Воскресенье) в текст.
--    Использование: WEEKDAY() возвращает 0 для понедельника, поэтому передавать
--    следует WEEKDAY(дата) + 1.
CREATE FUNCTION get_day_name(day_num INT)
RETURNS VARCHAR(20)
DETERMINISTIC
BEGIN
    RETURN CASE day_num
        WHEN 1 THEN 'Понедельник'
        WHEN 2 THEN 'Вторник'
        WHEN 3 THEN 'Среда'
        WHEN 4 THEN 'Четверг'
        WHEN 5 THEN 'Пятница'
        WHEN 6 THEN 'Суббота'
        WHEN 7 THEN 'Воскресенье'
        ELSE        'Неизвестно'
    END;
END$$

-- peek2
-- 5. Курсор для вывода записей из таблицы, удовлетворяющих заданному условию.
--    Процедура обходит единицы хранения, у которых количество превышает
--    переданный порог, и возвращает их в виде результирующего набора.
CREATE PROCEDURE get_large_orders(IN p_min_quantity DECIMAL(15,3))
BEGIN
    DECLARE v_storage_id    INT;
    DECLARE v_order_number  VARCHAR(50);
    DECLARE v_order_date    DATE;
    DECLARE v_material_name VARCHAR(255);
    DECLARE v_quantity      DECIMAL(15,3);
    DECLARE v_price         DECIMAL(15,2);
    DECLARE done            INT DEFAULT FALSE;

    DECLARE cur CURSOR FOR
        SELECT su.storage_id,
               su.order_number,
               su.order_date,
               m.material_name,
               su.quantity,
               su.price
        FROM storage_units su
        JOIN materials m ON su.material_id = m.material_id
        WHERE su.quantity > p_min_quantity;

    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;

    DROP TEMPORARY TABLE IF EXISTS tmp_large_orders;
    CREATE TEMPORARY TABLE tmp_large_orders (
        storage_id    INT,
        order_number  VARCHAR(50),
        order_date    DATE,
        material_name VARCHAR(255),
        quantity      DECIMAL(15,3),
        price         DECIMAL(15,2)
    );

    OPEN cur;
    read_loop: LOOP
        FETCH cur INTO v_storage_id, v_order_number, v_order_date,
                       v_material_name, v_quantity, v_price;
        IF done THEN
            LEAVE read_loop;
        END IF;
        INSERT INTO tmp_large_orders
        VALUES (v_storage_id, v_order_number, v_order_date,
                v_material_name, v_quantity, v_price);
    END LOOP;
    CLOSE cur;

    SELECT * FROM tmp_large_orders;
    DROP TEMPORARY TABLE IF EXISTS tmp_large_orders;
END$$

-- 6. Хранимая процедура, которая записывает в новую таблицу (storage_units_archive)
--    все кортежи из storage_units, относящиеся к заданному поставщику.
--    Процедура использует курсор, читающий данные строка за строкой в цикле.
CREATE PROCEDURE archive_supplier_orders(IN p_supplier_id INT)
BEGIN
    DECLARE v_order_number     VARCHAR(50);
    DECLARE v_order_date       DATE;
    DECLARE v_supplier_id      INT;
    DECLARE v_balance_account  VARCHAR(50);
    DECLARE v_document_type_id INT;
    DECLARE v_document_number  VARCHAR(50);
    DECLARE v_material_id      INT;
    DECLARE v_material_account VARCHAR(50);
    DECLARE v_unit_id          INT;
    DECLARE v_quantity         DECIMAL(15,3);
    DECLARE v_price            DECIMAL(15,2);
    DECLARE done               INT DEFAULT FALSE;

    DECLARE cur CURSOR FOR
        SELECT order_number, order_date, supplier_id, balance_account,
               document_type_id, document_number, material_id, material_account,
               unit_id, quantity, price
        FROM storage_units
        WHERE supplier_id = p_supplier_id;

    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;

    OPEN cur;
    read_loop: LOOP
        FETCH cur INTO v_order_number, v_order_date, v_supplier_id,
                       v_balance_account, v_document_type_id, v_document_number,
                       v_material_id, v_material_account, v_unit_id, v_quantity, v_price;
        IF done THEN
            LEAVE read_loop;
        END IF;
        INSERT INTO storage_units_archive
            (order_number, order_date, supplier_id, balance_account,
             document_type_id, document_number, material_id, material_account,
             unit_id, quantity, price)
        VALUES
            (v_order_number, v_order_date, v_supplier_id, v_balance_account,
             v_document_type_id, v_document_number, v_material_id, v_material_account,
             v_unit_id, v_quantity, v_price);
    END LOOP;
    CLOSE cur;
END$$

DELIMITER ;

-- Примеры вызовов

-- 1. Арифметика над полями: итоговая стоимость заказов поставщика с id = 1
-- CALL get_supplier_total_cost(1);

-- 2. Связанные записи из нескольких таблиц: полные сведения по заказам поставщика 1
-- CALL get_storage_units_full(1);

-- 3. Агрегаты: статистика цен для материала с id = 1 (Steel)
-- CALL get_material_price_stats(1);

-- 4. Функция с CASE: вывести название дня недели для каждой даты заказа
-- SELECT order_number, order_date, get_day_name(WEEKDAY(order_date) + 1) AS day_of_week FROM storage_units;

-- 5. Процедура с курсором: заказы, где количество > 1000
-- CALL get_large_orders(1000);

-- 6. Процедура с курсором-копированием: архивировать все заказы поставщика 1
-- CALL archive_supplier_orders(1);
-- SELECT * FROM storage_units_archive;
