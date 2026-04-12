# Интернет-магазин на микросервисной архитектуре

Этот проект представляет собой простой интернет-магазин, построенный на микросервисной архитектуре с использованием C# и ASP.NET Core. Включает сервисы для продуктов, заказов, пользователей, API Gateway и простой фронтенд на Blazor.

## Архитектура

- **ProductService** (порт 5001): Управление продуктами (CRUD).
- **OrderService** (порт 5002): Управление заказами (CRUD).
- **UserService** (порт 5003): Управление пользователями (CRUD).
- **ApiGateway** (порт 5000): API Gateway для проксирования запросов к сервисам.
- **Frontend** (Blazor WASM): Простой фронтенд для просмотра продуктов.

## Запуск

1. Запустите каждый микросервис в отдельном терминале:

   ```bash
   cd ProductService/ProductService && dotnet run
   cd OrderService/OrderService && dotnet run
   cd UserService/UserService && dotnet run
   cd ApiGateway/ApiGateway && dotnet run
   cd Frontend/Frontend && dotnet run
   ```

2. Откройте браузер и перейдите на http://localhost:5004 (или порт, указанный для Frontend).

3. Перейдите на страницу Products для просмотра списка продуктов.

## Минимальный код

Проект использует минимальный код: in-memory хранилища, базовые CRUD операции, простые модели.
