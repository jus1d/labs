USE warehouse;

-- Задание 4: добавление JSON-поля в таблицу suppliers.
--   extra_info содержит:
--     - вложенную структуру "contacts" (phone, email, manager),
--     - массив "certificates".
ALTER TABLE suppliers ADD COLUMN IF NOT EXISTS extra_info JSON;

UPDATE suppliers
SET extra_info = JSON_OBJECT(
    'contacts', JSON_OBJECT(
        'phone',   '+7-495-123-4567',
        'email',   'info@bibaboba.ru',
        'manager', 'Иванов Иван'
    ),
    'certificates', JSON_ARRAY('ISO 9001', 'GOST R 51901')
)
WHERE supplier_id = 1;

UPDATE suppliers
SET extra_info = JSON_OBJECT(
    'contacts', JSON_OBJECT(
        'phone',   '+7-812-987-6543',
        'email',   'supply@bbbbiwyimr.ru',
        'manager', 'Петров Алексей'
    ),
    'certificates', JSON_ARRAY('ISO 14001')
)
WHERE supplier_id = 2;

UPDATE suppliers
SET extra_info = JSON_OBJECT(
    'contacts', JSON_OBJECT(
        'phone',   '+7-383-112-2334',
        'email',   'office@buildfreak.ru',
        'manager', 'Сидоров Пётр'
    ),
    'certificates', JSON_ARRAY('ISO 9001', 'ISO 45001', 'GOST R 12.0.230')
)
WHERE supplier_id = 3;

-- Задание 5: запрос, возвращающий записи по условию на атрибут вложенной структуры.
--   Найти всех поставщиков, чей менеджер — 'Иванов Иван'.
SELECT
    supplier_id,
    supplier_name,
    extra_info ->>'$.contacts.manager' AS manager,
    extra_info ->>'$.contacts.email'   AS email,
    extra_info ->>'$.contacts.phone'   AS phone
FROM suppliers
WHERE extra_info ->>'$.contacts.manager' = 'Иванов Иван';

-- Задание 6: запрос, изменяющий значение по существующему ключу в заданной строке.
--   Обновить номер телефона поставщика с id = 1.
UPDATE suppliers
SET extra_info = JSON_SET(extra_info, '$.contacts.phone', '+7-495-000-0000')
WHERE supplier_id = 1;

-- Проверка результата
-- SELECT supplier_id, supplier_name, extra_info FROM suppliers;
