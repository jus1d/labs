INSERT INTO addresses (postal_index, city, street, house) VALUES
('101000', 'Moscow', 'Tverskaya', '10'),
('101000', 'Moscow', 'Lenina', '5'),
('190000', 'Saint-Petersburg', 'Nevskiy', '15'),
('190000', 'Saint-Petersburg', 'Marata', '7'),
('630000', 'Novosibirsk', 'Red ave.', '20'),
('630000', 'Novosibirsk', 'Sovetskaya', '3');

INSERT INTO suppliers (supplier_name, inn, legal_address_id, bank_address_id, bank_account) VALUES
('ООО МеталлПоставка', '7701234567', 1, 2, '40702810900000000001'),
('ООО ХимСнаб', '7809876543', 3, 4, '40702810900000000002'),
('ООО СтройРесурс', '5401122334', 5, 6, '40702810900000000003');

INSERT INTO material_classes (class_code) VALUES
('A'),
('B'),
('C');

INSERT INTO material_groups (group_code, class_id) VALUES
('A1', 1),
('A2', 1),
('B1', 2),
('C1', 3);

INSERT INTO materials (group_id, material_name) VALUES
(1, 'Steel'),
(2, 'Armetura'),
(3, 'Paint'),
(4, 'Gas 95');

INSERT INTO units (unit_name) VALUES
('kg'),
('leters');

INSERT INTO material_units (material_id, unit_id) VALUES
(1, 1),
(2, 1),
(3, 2),
(4, 2);

INSERT INTO document_types (document_name) VALUES
('Nakladnaya'),
('Factura');

INSERT INTO storage_units
(order_number, order_date, supplier_id, balance_account,
 document_type_id, document_number,
 material_id, material_account,
 unit_id, quantity, price)
VALUES
('ORD-001', CURDATE() - INTERVAL 10 DAY, 1, '10-01', 1, 'DOC-001', 1, '41-01', 1, 1000, 75.50),
('ORD-002', CURDATE() - INTERVAL 5 DAY, 2, '10-01', 2, 'DOC-002', 3, '41-02', 2, 500, 120.00),
('ORD-003', CURDATE() - INTERVAL 15 DAY, 1, '10-01', 1, 'DOC-003', 2, '41-01', 1, 2000, 60.00),
('ORD-004', CURDATE() - INTERVAL 20 DAY, 3, '10-01', 2, 'DOC-004', 1, '41-01', 1, 1500, 74.00),
('ORD-005', CURDATE() - INTERVAL 3 MONTH, 2, '10-01', 1, 'DOC-005', 4, '41-03', 2, 3000, 55.00);
