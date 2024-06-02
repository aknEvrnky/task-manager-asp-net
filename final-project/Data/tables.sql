CREATE TABLE customers (
                           id INT PRIMARY KEY IDENTITY,
                           name VARCHAR(100) NOT NULL,
                           email VARCHAR(100) NOT NULL UNIQUE,
                           phone VARCHAR(20),
                           created_at DATETIME DEFAULT GETDATE(),
                           updated_at DATETIME DEFAULT GETDATE()
);

CREATE TABLE users (
                       id INT PRIMARY KEY IDENTITY,
                       name VARCHAR(50) NOT NULL,
                       email VARCHAR(100) NOT NULL UNIQUE,
                       password VARCHAR(256) NOT NULL,
                       created_at DATETIME DEFAULT GETDATE(),
                       updated_at DATETIME DEFAULT GETDATE()
);

CREATE TABLE tasks (
                       id INT PRIMARY KEY IDENTITY,
                       customer_id INT FOREIGN KEY REFERENCES customers(id),
                       user_id INT FOREIGN KEY REFERENCES users(id),
                       title VARCHAR(100) NOT NULL,
                       content TEXT,
                       status VARCHAR(50),
                       priority VARCHAR(50),
                       started_at DATETIME DEFAULT GETDATE(),
                       finished_at DATETIME DEFAULT GETDATE(),
                       created_at DATETIME DEFAULT GETDATE(),
                       updated_at DATETIME DEFAULT GETDATE()

);