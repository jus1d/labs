USE warehouse;

-- Таблица аудита изменений единиц хранения (используется в задании 2)
CREATE TABLE IF NOT EXISTS storage_units_audit (
    audit_id     INT PRIMARY KEY AUTO_INCREMENT,
    operation    ENUM('INSERT', 'UPDATE', 'DELETE') NOT NULL,
    storage_id   INT,
    order_number VARCHAR(50),
    supplier_id  INT,
    material_id  INT,
    quantity     DECIMAL(15,3),
    price        DECIMAL(15,2),
    changed_at   DATETIME     DEFAULT CURRENT_TIMESTAMP,
    changed_by   VARCHAR(100) DEFAULT (CURRENT_USER())
);

DELIMITER $$

-- Задание 1a + 1b + 3: триггер BEFORE INSERT на storage_units.
--   1a. Запрещает вставку строки с количеством или ценой <= 0.
--   1b. Автоматически заполняет поле material_account на основе material_id,
--       если оно не было передано явно.
--   3.  Запрещает вставку с дублирующим номером ордера.
CREATE TRIGGER trg_storage_units_before_insert
BEFORE INSERT ON storage_units
FOR EACH ROW
BEGIN
    -- 1a: проверка корректности числовых полей
    IF NEW.quantity <= 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Количество должно быть положительным числом';
    END IF;
    IF NEW.price <= 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Цена должна быть положительным числом';
    END IF;

    -- 1b: автозаполнение material_account
    IF NEW.material_account IS NULL OR NEW.material_account = '' THEN
        SET NEW.material_account = CONCAT('ACC-', NEW.material_id);
    END IF;

    -- 3: запрет дублирующего номера ордера
    IF EXISTS (
        SELECT 1 FROM storage_units WHERE order_number = NEW.order_number
    ) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Единица хранения с таким номером ордера уже существует';
    END IF;
END$$

-- Задание 1a + 3: триггер BEFORE UPDATE на storage_units.
--   Повторяет проверки на корректность данных и уникальность номера ордера
--   при изменении записи.
CREATE TRIGGER trg_storage_units_before_update
BEFORE UPDATE ON storage_units
FOR EACH ROW
BEGIN
    -- 1a: проверка корректности числовых полей
    IF NEW.quantity <= 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Количество должно быть положительным числом';
    END IF;
    IF NEW.price <= 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Цена должна быть положительным числом';
    END IF;

    -- 3: запрет дублирующего номера ордера (исключаем текущую строку)
    IF NEW.order_number <> OLD.order_number AND EXISTS (
        SELECT 1 FROM storage_units
        WHERE order_number = NEW.order_number
          AND storage_id   <> OLD.storage_id
    ) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Единица хранения с таким номером ордера уже существует';
    END IF;
END$$

-- Задание 2: триггер AFTER INSERT — запись в аудит при добавлении строки.
CREATE TRIGGER trg_storage_units_audit_insert
AFTER INSERT ON storage_units
FOR EACH ROW
BEGIN
    INSERT INTO storage_units_audit
        (operation, storage_id, order_number, supplier_id, material_id, quantity, price)
    VALUES
        ('INSERT', NEW.storage_id, NEW.order_number, NEW.supplier_id,
         NEW.material_id, NEW.quantity, NEW.price);
END$$

-- Задание 2: триггер AFTER UPDATE — запись в аудит при изменении строки.
CREATE TRIGGER trg_storage_units_audit_update
AFTER UPDATE ON storage_units
FOR EACH ROW
BEGIN
    INSERT INTO storage_units_audit
        (operation, storage_id, order_number, supplier_id, material_id, quantity, price)
    VALUES
        ('UPDATE', NEW.storage_id, NEW.order_number, NEW.supplier_id,
         NEW.material_id, NEW.quantity, NEW.price);
END$$

-- Задание 2: триггер AFTER DELETE — запись в аудит при удалении строки.
CREATE TRIGGER trg_storage_units_audit_delete
AFTER DELETE ON storage_units
FOR EACH ROW
BEGIN
    INSERT INTO storage_units_audit
        (operation, storage_id, order_number, supplier_id, material_id, quantity, price)
    VALUES
        ('DELETE', OLD.storage_id, OLD.order_number, OLD.supplier_id,
         OLD.material_id, OLD.quantity, OLD.price);
END$$

-- Задание 3: триггер BEFORE INSERT на materials — запрет дублирующего наименования.
CREATE TRIGGER trg_materials_before_insert
BEFORE INSERT ON materials
FOR EACH ROW
BEGIN
    IF EXISTS (
        SELECT 1 FROM materials WHERE material_name = NEW.material_name
    ) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Материал с таким наименованием уже существует';
    END IF;
END$$

-- Задание 3: триггер BEFORE UPDATE на materials — запрет дублирующего наименования.
CREATE TRIGGER trg_materials_before_update
BEFORE UPDATE ON materials
FOR EACH ROW
BEGIN
    IF NEW.material_name <> OLD.material_name AND EXISTS (
        SELECT 1 FROM materials
        WHERE material_name = NEW.material_name
          AND material_id   <> OLD.material_id
    ) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Материал с таким наименованием уже существует';
    END IF;
END$$

DELIMITER ;

-- Примеры вызовов

-- Успешная вставка (material_account будет заполнен автоматически как 'ACC-1')
-- INSERT INTO storage_units (order_number, order_date, supplier_id, balance_account,
--     document_type_id, document_number, material_id, unit_id, quantity, price)
-- VALUES ('ORD-010', CURDATE(), 1, '10-01', 1, 'DOC-010', 1, 1, 500, 80.00);

-- Ошибка: количество <= 0
-- INSERT INTO storage_units (order_number, order_date, supplier_id, balance_account,
--     document_type_id, document_number, material_id, unit_id, quantity, price)
-- VALUES ('ORD-011', CURDATE(), 1, '10-01', 1, 'DOC-011', 1, 1, -10, 80.00);

-- Ошибка: дублирующий номер ордера
-- INSERT INTO storage_units (order_number, order_date, supplier_id, balance_account,
--     document_type_id, document_number, material_id, unit_id, quantity, price)
-- VALUES ('ORD-001', CURDATE(), 1, '10-01', 1, 'DOC-012', 1, 1, 100, 80.00);

-- Ошибка: дублирующее наименование материала
-- INSERT INTO materials (group_id, material_name) VALUES (1, 'Steel');

-- Просмотр журнала аудита
-- SELECT * FROM storage_units_audit ORDER BY changed_at DESC;
