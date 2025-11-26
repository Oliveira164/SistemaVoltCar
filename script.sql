CREATE DATABASE dbSistemaVoltCar;

USE dbSistemaVoltCar;

CREATE TABLE Usuario (
    IdUsuario INT PRIMARY KEY AUTO_INCREMENT,
    Nome VARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    Senha VARCHAR(8) NOT NULL
);

CREATE TABLE Fornecedor (
    IdFornecedor INT PRIMARY KEY AUTO_INCREMENT,
    Nome VARCHAR(100) NOT NULL,
    CNPJ BIGINT(14) UNIQUE NOT NULL,
    Telefone Decimal(11,0)
);

CREATE TABLE Veiculo (
    IdVeiculo INT PRIMARY KEY AUTO_INCREMENT,
    Modelo VARCHAR(50) NOT NULL,
    Marca VARCHAR(50) NOT NULL,
    Ano INT,
    Valor DECIMAL(10,2),
    IdFornecedor INT,
    FOREIGN KEY (IdFornecedor) REFERENCES Fornecedor(IdFornecedor)
);

SELECT * FROM Usuario;

