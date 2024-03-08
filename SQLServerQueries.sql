-- CREATE DATABASE Practicas;
-- USE Practicas;
DROP TABLE Users;
CREATE TABLE Users (email VARCHAR(255) PRIMARY KEY, name VARCHAR(255) NOT NULL, password VARCHAR(255) NOT NULL, birthdate DATE NOT NULL);
INSERT INTO Users (name, email, password, birthdate) VALUES ('David','d@d.com','ddd','2002-03-06');
INSERT INTO Users (name, email, password, birthdate) VALUES ('Oscar','o@o.com','ooo','2002-02-05'),('Eduardo','e@e.com','eee','2002-04-07'),('Roberto','r@r.com','rrr','2002-06-09');
SELECT * FROM Users;
