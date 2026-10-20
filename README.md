# НИКОМУ НЕ НУЖНЫЙ ФАЙЛ
Здравствуйте, кто бы то ни был. Это кроссплатформер под дипломную работу (создание виртуального приложения для практикумов по химии). Я пока не знаю, поможет ли он для моей дипломной работы, поскольку у меня проект вроде должен быть в формате exe (приложения), но я по крайней мере сделаю хоть что-то, что может пригодиться как план Б.

Что есть? Есть  пока только бэк, где пишется основная архитектура. Модели: реагенты, установки, сессии и шаги. Также для них есть контроллеры. Также есть и контроллер авторизации (user admin). Роли определяются по токенам. Admin может добавлять данные (реагенты и стенды). User не может.

4 БДшки. Для начала пользователь открывает сессию, затем выполняет шаг (отправляется в виде JSON формата). Если есть ошибка (IsSuccess = 0) - вычет 30 баллов. Меньше 50 из 100 за общую работу (сессию) - незачёт (IsPassed = 0).

Запросы Postman:

Базовый URL для всех эндпоинтов: `{{baseUrl}}` (например, `http://localhost:5065`)

### 1. Авторизация
* **GET 1.1 Получить токен (User)**
  * `GET {{baseUrl}}/api/Auth/get-token?role=User`
* **GET 1.2 Получить токен (Admin)**
  * `GET {{baseUrl}}/api/Auth/get-token?role=Admin`

### 2. Справочники и оборудование (Общедоступные)
* **GET Список реактивов (БД)**
  * `GET {{baseUrl}}/api/Reagents`
* **GET Состояние оборудования и загруженных реагентов**
  * `GET {{baseUrl}}/api/Lab/apparatuses`

### 3. Ход лабораторного практикума (Требуется токен)
* **POST Создать сессию**
  * `POST {{baseUrl}}/api/Sessions/start?studentName=Артем%20Хабаров&experimentTitle=Опыт1`
  * *Headers:* `Authorization: Bearer <token>`
* **POST Шаг опыта (Успешно)**
  * `POST {{baseUrl}}/api/Steps/session/{sessionId}/perform`
  * *Headers:* `Authorization: Bearer <token>`
  * *Body (JSON):*
    ```json
    {
      "actionCode": "HEAT_AND_DISSOLVE",
      "reactionFormula": "C17H35COONa + H-O-H -> C17H35COOH + NaOH",
      "resultOutput": "Мыло быстро растворилось при нагреве. Среда щелочная.",
      "isSuccess": true
    }
    ```
* **POST Шаг опыта (С ошибкой / Штрафом)**
  * `POST {{baseUrl}}/api/Steps/session/{sessionId}/perform`
  * *Headers:* `Authorization: Bearer <token>`
  * *Body (JSON):*
    ```json
    {
      "actionCode": "OVERHEAT_DANGER",
      "reactionFormula": "N/A",
      "resultOutput": "Ошибка! Допущено закипание и разбрызгивание щелочи.",
      "isSuccess": false
    }
    ```
* **POST Завершить сессию**
  * `POST {{baseUrl}}/api/Sessions/{sessionId}/finish`
  * *Headers:* `Authorization: Bearer <token>`
  * *Правила оценки:* За каждый ошибочный шаг (`IsSuccess = false`) начисляется штраф -30 баллов. Если итоговый балл падает ниже 50, работа получает статус незачета (`IsPassed = false`).


* **GET Успешные сессии (Аналитика LINQ)**
  * `GET {{baseUrl}}/api/Sessions/analytics`
* **POST Добавить новый реактив (Только для Admin)**
  * `POST {{baseUrl}}/api/Reagents`
  * *Headers:* `Authorization: Bearer <admin_token>`
  * *Body (JSON):*
    ```json
    {
      "name": "Соляная кислота",
      "chemicalFormula": "HCl",
      "stateOfMatter": "Liquid",
      "hazardLevel": 3
    }
    ```
* **POST Добавить лабораторную установку (Только для Admin)**
  * `POST {{baseUrl}}/api/Lab/apparatus`
  * *Headers:* `Authorization: Bearer <admin_token>`
  * *Body (JSON):*
    ```json
    {
      "apparatusType": "Электропечь лабораторная",
      "isHeaterOn": false,
      "currentTemperatureC": 20.0,
      "volumeMl": 250.0,
      "loadedReagentId": null
    }
    ```
