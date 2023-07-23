# Practice_Task_1
_Студент ТулГУ Петина Полина, гр. 221111_
## Текст задания
Каталог услуг управляющей компании

Необходимо создать каталог услуг упавляющей компании. Сущности:

* Управляющая компания:

  * id
  * название
 
* Локация (зона действия)

  * id
  * название
  * id управляющей компании (foreign key на таблицу управляющих компаний)
 
* Адрес в локации:

  * код дома ФИАС
  * id локации (foreign key на таблицу локаций)

* Позиция каталога (услуга):

  * id
  * название
  * минимальное количество элементов в заказе
  * максимаальное количество элементов в заказе
  * единицы измерения (просто строка)
  * id управляющей компании
 
* Цена по локации:

  * id
  * цена (целое число, в копейках)
  * признак "цена по запросу" (если true, то цена равна 0)
  * id локации (foreign key на таблицу локаций)
  * id позиции каталога (foreign key на таблицу услуг)

Необходимо реализовать REST API со следующими функциями

* Добавление, удаление, изменение позиций каталога
* Добавление, удаление, изменение цен по локации
* Получение цены по набру параметров: код дома ФИАС + id позиции каталога

## Код для создания БД (PosgresSQL)
```
-- Управляющая компания
CREATE TABLE  Company (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL
);

-- Локация
CREATE TABLE Location (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    company_id INTEGER REFERENCES  Company(id)
)

-- Адрес в локации
CREATE TABLE Address_in_Location (
    fias_house_code VARCHAR(32) NOT NULL,
    location_id INTEGER REFERENCES Location(id)
);

-- Каталог
CREATE TABLE Catalog (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    min_order INTEGER NOT NULL,
    max_order INTEGER NOT NULL,
    measurement VARCHAR(255) NOT NULL,
    company_id INTEGER REFERENCES  Company(id)
);

-- Цена по локации
CREATE TABLE Price_by_Location (
    id SERIAL PRIMARY KEY,
    price INTEGER NOT NULL,
    price_on_request BOOLEAN NOT NULL,
    location_id INTEGER REFERENCES Location(id),
    catalog_id INTEGER REFERENCES Catalog(id)
);
```
Подключение к БД задается в файле **appsettings.json**

В проекте использовались следующие пакеты:
```
 Microsoft.EntityFrameworkCore
 Npgsql.EntityFrameworkCore.PostgreSQL 
 
```
