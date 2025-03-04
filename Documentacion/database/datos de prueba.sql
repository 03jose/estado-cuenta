
------ valores de prueba
INSERT INTO Cliente (ClienteId, Nombre, Apellido, DUI) VALUES
(1,	'juan',	'perez',	'07834567-7'),
(2,	'jose Ricardo',	'martinez ruano',	'00042315-1'),
(3, 'Carlos', 'Pérez', '01234567-8'),
(4, 'Ana', 'Gómez', '12345678-9'),
(5, 'Luis', 'Rodríguez', '23456789-0'),
(6, 'María', 'Hernández', '34567890-1'),
(7, 'José', 'Martínez', '45678901-2'),
(8, 'Sofía', 'López', '56789012-3'),
(9, 'Daniel', 'Ramírez', '67890123-4'),
(10, 'Elena', 'Torres', '78901234-5'),
(11, 'Pedro', 'Ortega', '89012345-6'),
(12, 'Gabriela', 'Morales', '90123456-7');

INSERT INTO TarjetaCredito (Cliente_Id, Numero_Tarjeta, Limite_Credito, Saldo_Utilizado, TasaInteresConfigurable, Fecha_Corte, Fecha_Pago, porcentaje_Saldo_min) VALUES
(1, 3, '4111111111111111', 5000.00, 1200.50, 2.5, '2025-03-10', '2025-03-25', 10.00),
(2, 4, '4222222222222222', 3000.00, 800.00, 3.0, '2025-03-15', '2025-03-30', 12.00),
(3, 5, '4333333333333333', 10000.00, 2500.75, 1.8, '2025-03-05', '2025-03-20', 8.00),
(4, 6, '4444444444444444', 7000.00, 1500.20, 2.0, '2025-03-12', '2025-03-27', 9.50),
(5, 7, '4555555555555555', 2000.00, 500.00, 3.5, '2025-03-18', '2025-04-02', 11.00);

INSERT INTO MovimientoTarjeta (TarjetaCredito_ID, Fecha_Movimiento, Descripcion, Monto, Tipo_Movimiento) VALUES
(1, 1, '2025-02-20', 'Compra en supermercado', 150.75, 'compra'),
(2, 1, '2025-02-28', 'Pago de tarjeta', 500.00, 'pago'),
(3, 2, '2025-03-01', 'Compra en tienda electrónica', 250.00, 'compra'),
(4, 2, '2025-03-05', 'Pago de tarjeta', 300.00, 'pago'),
(5, 3, '2025-02-15', 'Compra en restaurante', 80.50, 'compra'),
(6, 3, '2025-02-22', 'Pago de tarjeta', 1000.00, 'pago'),
(7, 4, '2025-02-10', 'Compra en gasolinera', 45.00, 'compra'),
(8, 4, '2025-02-28', 'Pago de tarjeta', 600.00, 'pago'),
(9, 5, '2025-03-02', 'Compra en tienda de ropa', 120.00, 'compra'),
(10, 5, '2025-03-08', 'Pago de tarjeta', 200.00, 'pago');
