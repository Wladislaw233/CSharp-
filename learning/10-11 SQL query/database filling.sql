INSERT INTO client (first_name, last_name, date_of_birth, age, address, email, phone_number)
VALUES ('Rod', 'Barton', date('01.05.1983'), 40, 'Каменка, ул. Мира', 'RodBarton@example.com', '48617499'),
       ('Eli', 'Chandler', date('23.10.1993 0:00:00'), 29, 'Тирасполь, ул. Лесная', 'EliChandler@yahoo.com',
        '52358357'),
       ('Ralph', 'Chandler', date('26.12.1995 0:00:00'), 27, 'Рыбница, ул. Мира', 'RalphChandler@example.com',
        '41048680'),
       ('Buck', 'Paul', date('18.05.1970 0:00:00'), 53, 'Дубоссары, пер. Энгельса', 'BuckPaul@hotmail.com', '81037582'),
       ('Benita', 'Chandler', date('12.02.1991 0:00:00'), 32, 'Рыбница, ул. 25 Октября', 'BenitaChandler@outlook.com',
        '81678312'),
       ('Quinn', 'Christian', date('19.04.1962 0:00:00'), 61, 'Каменка, ул. Мира', 'QuinnChristian@outlook.com',
        '26779450');

INSERT INTO employee (first_name, last_name, date_of_birth, age, contract, salary, address, email, phone_number)
VALUES ('Barbra', 'Barton', date('03.07.1956 0:00:00'), 67, 'Barbra Barton, дата рождения: 03.07.1956 0:00:00', 47241,
        'Каменка, ул. Лесная', 'BarbraBarton@testmail.com', '59771354'),
       ('Kirby', 'Stout', date('23.08.2003 0:00:00'), 19, 'Kirby Stout, дата рождения: 23.08.2003 0:00:00', 27801,
        'Рыбница, пер. Энгельса', 'KirbyStout@example.com', '18925017'),
       ('Colette', 'Zhang', date('17.03.1955 0:00:00'), 68, 'Colette Zhang, дата рождения: 17.03.1955 0:00:00', 22863,
        'Каменка, пер. Энгельса', 'ColetteZhang@hotmail.com', '52394175'),
       ('Ralph', 'Bright', date('13.10.1998 0:00:00'), 24, 'Ralph Bright, дата рождения: 13.10.1998 0:00:00', 44469,
        'Григориополь, пер. Энгельса', 'RalphBright@yahoo.com', '86317246'),
       ('Kirby', 'Collins', date('05.02.1995 0:00:00'), 28, 'Kirby Collins, дата рождения: 05.02.1995 0:00:00', 85436,
        'Тирасполь, ул. Лесная', 'KirbyCollins@example.com', '29433093'),
       ('Barbra', 'Tapia', date('19.05.1961 0:00:00'), 62, 'Barbra Tapia, дата рождения: 19.05.1961 0:00:00', 51042,
        'Каменка, ул. 25 Октября', 'BarbraTapia@hotmail.com', '87124309');

INSERT INTO account(amount, client_id)
SELECT FLOOR(random() * 100000), id
FROM client;