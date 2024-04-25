-- Create Patient table
CREATE TABLE Patient (
    id_patient VARCHAR(32) PRIMARY KEY,
    name VARCHAR(255)
);

-- Create Tooth table
CREATE TABLE Tooth (
    id_tooth VARCHAR(32) PRIMARY KEY,
    replacement_price DOUBLE PRECISION,
    cleaning_price DOUBLE PRECISION,
    repair_price DOUBLE PRECISION,
    removal_price DOUBLE PRECISION,
    beauty_priority INT,
    health_priority INT
);
INSERT INTO Tooth (id_tooth, replacement_price, cleaning_price, repair_price, removal_price, beauty_priority, health_priority)
VALUES 
    ('H1', 100000, 1000, 2000, 5000, 1, 32),
    ('H2', 100000, 1000, 2000, 5000, 2, 31),
    ('H3', 100000, 1000, 2000, 5000, 3, 30),
    ('H4', 100000, 1000, 2000, 5000, 4, 29),
    ('H5', 100000, 1000, 2000, 5000, 5, 28),
    ('H6', 100000, 1000, 2000, 5000, 6, 27),
    ('H7', 100000, 1000, 2000, 5000, 7, 26),
    ('H8', 100000, 1000, 2000, 5000, 8, 25),
    ('H9', 100000, 1000, 2000, 5000, 9, 24),
    ('H10', 100000, 1000, 2000, 5000, 10, 23),
    ('H11', 100000, 1000, 2000, 5000, 11, 22),
    ('H12', 100000, 1000, 2000, 5000, 12, 21),
    ('H13', 100000, 1000, 2000, 5000, 13, 20),
    ('H14', 100000, 1000, 2000, 5000, 14, 19),
    ('H15', 100000, 1000, 2000, 5000, 15, 18),
    ('H16', 100000, 1000, 2000, 5000, 16, 17),
    ('B1', 100000, 1000, 2000, 5000, 17, 16),
    ('B2', 100000, 1000, 2000, 5000, 18, 15),
    ('B3', 100000, 1000, 2000, 5000, 19, 14),
    ('B4', 100000, 1000, 2000, 5000, 20, 13),
    ('B5', 100000, 1000, 2000, 5000, 21, 12),
    ('B6', 100000, 1000, 2000, 5000, 22, 11),
    ('B7', 100000, 1000, 2000, 5000, 23, 10),
    ('B8', 100000, 1000, 2000, 5000, 24, 9),
    ('B9', 100000, 1000, 2000, 5000, 25, 8),
    ('B10', 100000, 1000, 2000, 5000, 26, 7),
    ('B11', 100000, 1000, 2000, 5000, 27, 6),
    ('B12', 100000, 1000, 2000, 5000, 28, 5),
    ('B13', 100000, 1000, 2000, 5000, 29, 4),
    ('B14', 100000, 1000, 2000, 5000, 30, 3),
    ('B15', 100000, 1000, 2000, 5000, 31, 2),
    ('B16', 100000, 1000, 2000, 5000, 32, 1);


-- Create PatientTooth table
CREATE TABLE Patient_Tooth (
    id_patient VARCHAR(32),
    id_tooth VARCHAR(32),
    condition INT,
    date_visit DATE
);

delete from Patient;
delete from Tooth;
delete from Patient_Tooth;

CREATE TABLE IF NOT EXISTS action_table (
    id_patient VARCHAR(255) NOT NULL,
    id_tooth VARCHAR(255) NOT NULL,
    action VARCHAR(255) NOT NULL,
    depense DOUBLE PRECISION NOT NULL,
    reste DOUBLE PRECISION NOT NULL,
    condition DOUBLE PRECISION NOT NULL
);


-- Insert test data for Patient
INSERT INTO Patient (id_patient, name)
VALUES ('P1', 'Test Patient');

-- Insert test data for PatientTooth
INSERT INTO Patient_Tooth (id_patient, id_tooth, condition, date_visit)
VALUES 
    ('P1', 'H1', 1, '2024-01-07'),
    ('P1', 'H2', 5, '2024-01-08'),
    ('P1', 'H3', 10, '2024-01-09'),
    ('P1', 'H4', 6, '2024-01-10'),
    ('P1', 'H5', 8, '2024-01-11'),
    ('P1', 'H6', 9, '2024-01-12'),
    ('P1', 'H7', 5, '2024-01-13'),
    ('P1', 'H8', 10, '2024-01-14'),
    ('P1', 'H9', 10, '2024-01-15'),
    ('P1', 'H10',10, '2024-01-16');
