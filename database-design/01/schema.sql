CREATE DATABASE warehouse;
USE warehouse;

CREATE TABLE addresses (
    address_id INT PRIMARY KEY AUTO_INCREMENT,
    postal_index VARCHAR(10),
    city VARCHAR(100),
    street VARCHAR(150),
    house VARCHAR(20)
);

CREATE TABLE suppliers (
    supplier_id INT PRIMARY KEY AUTO_INCREMENT,
    supplier_name VARCHAR(255) NOT NULL,
    inn VARCHAR(12) NOT NULL,
    legal_address_id INT,
    bank_address_id INT,
    bank_account VARCHAR(30),
    FOREIGN KEY (legal_address_id) REFERENCES addresses(address_id),
    FOREIGN KEY (bank_address_id) REFERENCES addresses(address_id)
);

CREATE TABLE material_classes (
    class_id INT PRIMARY KEY AUTO_INCREMENT,
    class_code VARCHAR(50) NOT NULL
);

CREATE TABLE material_groups (
    group_id INT PRIMARY KEY AUTO_INCREMENT,
    group_code VARCHAR(50) NOT NULL,
    class_id INT NOT NULL,
    FOREIGN KEY (class_id) REFERENCES material_classes(class_id)
);

CREATE TABLE materials (
    material_id INT PRIMARY KEY AUTO_INCREMENT,
    group_id INT NOT NULL,
    material_name VARCHAR(255) NOT NULL,
    FOREIGN KEY (group_id) REFERENCES material_groups(group_id)
);

CREATE TABLE units (
    unit_id INT PRIMARY KEY AUTO_INCREMENT,
    unit_name VARCHAR(50) NOT NULL
);

CREATE TABLE material_units (
    id INT PRIMARY KEY AUTO_INCREMENT,
    material_id INT NOT NULL,
    unit_id INT NOT NULL,
    FOREIGN KEY (material_id) REFERENCES materials(material_id)
        ON DELETE CASCADE,
    FOREIGN KEY (unit_id) REFERENCES units(unit_id)
);

CREATE TABLE document_types (
    document_type_id INT PRIMARY KEY AUTO_INCREMENT,
    document_name VARCHAR(255) NOT NULL
);

CREATE TABLE storage_units (
    storage_id INT PRIMARY KEY AUTO_INCREMENT,
    order_number VARCHAR(50) NOT NULL,
    order_date DATE NOT NULL,
    supplier_id INT NOT NULL,
    balance_account VARCHAR(50),
    document_type_id INT,
    document_number VARCHAR(50),
    material_id INT NOT NULL,
    material_account VARCHAR(50),
    unit_id INT NOT NULL,
    quantity DECIMAL(15,3) NOT NULL,
    price DECIMAL(15,2) NOT NULL,

    FOREIGN KEY (supplier_id) REFERENCES suppliers(supplier_id),
    FOREIGN KEY (material_id) REFERENCES materials(material_id),
    FOREIGN KEY (document_type_id) REFERENCES document_types(document_type_id),
    FOREIGN KEY (unit_id) REFERENCES units(unit_id)
);
