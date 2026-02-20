-- cписок материалов, поставляемых определенным поставщиком
SELECT DISTINCT m.material_id, m.material_name
FROM storage_units su
JOIN materials m ON su.material_id = m.material_id
WHERE su.supplier_id = 1;

-- информация о 'единицах хранения', поступивших за последний месяц
SELECT su.*, u.unit_name
FROM storage_units su
JOIN units u ON su.unit_id = u.unit_id
WHERE su.order_date >= DATE_SUB(CURDATE(), INTERVAL 1 MONTH);

-- количество однотипного материала, поступившего за последний месяц
SELECT
    m.material_name,
    SUM(su.quantity) AS total_quantity
FROM storage_units su
JOIN materials m ON su.material_id = m.material_id
WHERE su.order_date >= DATE_SUB(CURDATE(), INTERVAL 1 MONTH)
GROUP BY m.material_name;

-- список материалов, единица измерения которых -- килограммы
SELECT DISTINCT m.material_id, m.material_name
FROM materials m
JOIN material_units mu ON m.material_id = mu.material_id
JOIN units u ON mu.unit_id = u.unit_id
WHERE u.unit_name = 'килограммы';

-- полная информация о поставщиках, поставляющих определенный вид материалов
SELECT DISTINCT
    s.supplier_id,
    s.supplier_name,
    s.inn,
    la.postal_index AS legal_index,
    la.city AS legal_city,
    la.street AS legal_street,
    la.house AS legal_house,
    ba.postal_index AS bank_index,
    ba.city AS bank_city,
    ba.street AS bank_street,
    ba.house AS bank_house,
    s.bank_account
FROM suppliers s
JOIN addresses la ON s.legal_address_id = la.address_id
JOIN addresses ba ON s.bank_address_id = ba.address_id
JOIN storage_units su ON s.supplier_id = su.supplier_id
WHERE su.material_id = 1;
