# 🛒 Microservice Shop — Интернет-магазин на микросервисной архитектуре

Привет! Это мой проект курсовой работы по Docker и Kubernetes.
Здесь реализован полноценный интернет-магазин, построенный на микросервисной архитектуре с использованием **C#** и **ASP.NET Core**.

Главная цель проекта — показать, как правильно контейнеризировать микросервисы, настраивать их взаимодействие через API Gateway и оркестрировать всё это через Docker Compose.

---

## 🏗 Архитектура проекта

Система состоит из 5 независимых компонентов, каждый из которых работает в своем Docker-контейнере:

### Микросервисы:

1. **ProductService** (порт 5001) 📦
   - Управление каталогом товаров
   - CRUD операции для продуктов
   - Хранение данных в in-memory коллекции (для демонстрации)

2. **OrderService** (порт 5002) 📋
   - Управление заказами
   - Создание, чтение, обновление, удаление заказов
   - Связь с пользователями и продуктами

3. **UserService** (порт 5003) 👤
   - Управление пользователями
   - Регистрация и аутентификация
   - Профили покупателей

4. **ApiGateway** (порт 5000) 🚪
   - Единая точка входа для всех запросов
   - Проксирование запросов к нужным микросервисам
   - Маршрутизация и балансировка нагрузки

5. **Frontend** (порт 5004) 💻
   - Веб-интерфейс на **Blazor WebAssembly**
   - Просмотр каталога товаров
   - Интерактивный UI без перезагрузки страниц

---

## 🛠 Технологический стек

### Backend
* **Язык:** C# 10+
* **Фреймворк:** ASP.NET Core 6/7
* **Архитектура:** Микросервисы, API Gateway pattern
* **Хранилище:** In-memory коллекции (упрощенная версия для демонстрации)

### Frontend
* **Фреймворк:** Blazor WebAssembly
* **Язык:** C# (да, фронтенд тоже на C#!)
* **UI:** Razor components

### Инфраструктура
* **Контейнеризация:** Docker с multi-stage builds
* **Оркестрация:** Docker Compose
* **Сеть:** Bridge network для изоляции сервисов
* **Health Checks:** Мониторинг состояния всех сервисов
* **Логирование:** JSON-file driver с ротацией логов

---

## 📂 Структура проекта

```text
Microservice/
├── ProductService/          # Микросервис товаров
│   └── ProductService/
│       ├── Controllers/     # API endpoints
│       ├── Models/          # Модели данных (Product)
│       ├── Program.cs       # Точка входа
│       ├── Dockerfile       # Контейнеризация сервиса
│       └── appsettings.json # Конфигурация
│
├── OrderService/            # Микросервис заказов
│   └── OrderService/
│       ├── Controllers/
│       ├── Models/          # Models (Order)
│       ├── Program.cs
│       ├── Dockerfile
│       └── appsettings.json
│
├── UserService/             # Микросервис пользователей
│   └── UserService/
│       ├── Controllers/
│       ├── Models/          # Models (User)
│       ├── Program.cs
│       ├── Dockerfile
│       └── appsettings.json
│
├── ApiGateway/              # API Gateway
│   └── ApiGateway/
│       ├── Controllers/     # Проксирование запросов
│       ├── Program.cs
│       ├── Dockerfile
│       └── appsettings.json
│
├── Frontend/                # Blazor WASM приложение
│   └── Frontend/
│       ├── Pages/           # Страницы приложения
│       ├── Shared/          # Общие компоненты
│       ├── wwwroot/         # Статические файлы
│       ├── Program.cs
│       ├── Dockerfile
│       └── nginx.conf       # Конфигурация nginx для SPA
│
├── docker-compose.yml       # Оркестрация всех сервисов
├── DOCKER_README.md         # Подробная документация по Docker
├── Microservice.sln         # Решение Visual Studio
└── README.md                # Ты тут :)
```

---

## 🚀 Как запустить проект

### Способ 1: Через Docker Compose (рекомендуется) ⭐

Это самый простой способ поднять всю систему одной командой:

```bash
# Клонируем репозиторий
git clone https://github.com/MrNikto1/Microservice.git
cd Microservice

# Запускаем все сервисы
docker-compose up --build

# Или в фоновом режиме
docker-compose up -d --build
```

Docker Compose автоматически:
- Соберет Docker-образы для каждого сервиса
- Создаст общую сеть `microservices-network`
- Запустит все 5 контейнеров с правильными зависимостями
- Настроит health checks для мониторинга

После запуска:
- **Frontend:** http://localhost:5004
- **API Gateway:** http://localhost:5000
- **ProductService:** http://localhost:5001
- **OrderService:** http://localhost:5002
- **UserService:** http://localhost:5003

### Способ 2: Локальный запуск (без Docker)

Если хочешь запустить сервисы напрямую через .NET CLI:

```bash
# Терминал 1 - ProductService
cd ProductService/ProductService
dotnet run

# Терминал 2 - OrderService
cd OrderService/OrderService
dotnet run

# Терминал 3 - UserService
cd UserService/UserService
dotnet run

# Терминал 4 - ApiGateway
cd ApiGateway/ApiGateway
dotnet run

# Терминал 5 - Frontend
cd Frontend/Frontend
dotnet run
```

⚠️ **Важно:** При локальном запуске нужно вручную настроить URL в конфигурации ApiGateway, чтобы он обращался к `localhost:500X` вместо внутренних Docker-имен сервисов.

---

## 🐳 Docker и контейнеризация

### Multi-stage builds

Каждый Dockerfile использует multi-stage build для оптимизации размера образа:

```dockerfile
# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish -c Release -o out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "ProductService.dll"]
```

Это позволяет:
- Уменьшить размер финального образа в 3-4 раза
- Убрать из продакшн-образа все инструменты сборки
- Ускорить деплой и снизить攻击面

### Docker Compose конфигурация

`docker-compose.yml` описывает:
- **Сервисы:** Все 5 микросервисов
- **Сети:** `microservices-network` для изоляции
- **Тома:** Persistent storage для данных
- **Зависимости:** `depends_on` для правильного порядка запуска
- **Health checks:** Автоматическая проверка состояния сервисов
- **Logging:** JSON-file driver с ротацией (10MB, 3 файла)

### Health Checks

Каждый сервис имеет endpoint `/health`, который Docker использует для проверки состояния:

```yaml
healthcheck:
  test: ["CMD", "curl", "-f", "http://localhost:5001/health"]
  interval: 30s
  timeout: 10s
  retries: 3
  start_period: 10s
```

Если сервис падает, Docker автоматически его перезапускает.

---

## 📡 API Endpoints

### ProductService (через API Gateway)

```
GET    /api/products          # Получить все товары
GET    /api/products/{id}     # Получить товар по ID
POST   /api/products          # Создать новый товар
PUT    /api/products/{id}     # Обновить товар
DELETE /api/products/{id}     # Удалить товар
```

### OrderService

```
GET    /api/orders            # Получить все заказы
GET    /api/orders/{id}       # Получить заказ по ID
POST   /api/orders            # Создать новый заказ
PUT    /api/orders/{id}       # Обновить заказ
DELETE /api/orders/{id}       # Удалить заказ
```

### UserService

```
GET    /api/users             # Получить всех пользователей
GET    /api/users/{id}        # Получить пользователя по ID
POST   /api/users             # Создать нового пользователя
PUT    /api/users/{id}        # Обновить пользователя
DELETE /api/users/{id}        # Удалить пользователя
```

---

## 🔧 Полезные команды Docker

```bash
# Посмотреть статус всех контейнеров
docker-compose ps

# Посмотреть логи всех сервисов
docker-compose logs

# Посмотреть логи конкретного сервиса
docker-compose logs -f product-service

# Остановить все сервисы
docker-compose down

# Остановить и удалить все volumes
docker-compose down -v

# Пересобрать образы после изменений в коде
docker-compose up --build

# Масштабирование (запустить 3 копии ProductService)
docker-compose up -d --scale product-service=3
```

---

## 🌐 Сетевая архитектура

Все сервисы работают в общей Docker-сети `microservices-network`:

```
┌─────────────────────────────────────────────────────┐
│              microservices-network                  │
│                                                     │
│  ┌──────────┐      ┌──────────────────┐            │
│  │ Frontend │─────▶│  ApiGateway      │            │
│  │  :5004   │      │    :5000         │            │
│  └──────────┘      └────────┬─────────┘            │
│                             │                       │
│              ┌──────────────┼──────────────┐       │
│              │              │              │       │
│              ▼              ▼              ▼       │
│     ┌────────────┐  ┌────────────┐  ┌────────┐    │
│     │ ProductService│ │OrderService│  │UserService│ │
│     │   :5001    │  │   :5002    │  │  :5003  │    │
│     └────────────┘  └────────────┘  └────────┘    │
│                                                     │
└─────────────────────────────────────────────────────┘
```

**Преимущества:**
- Сервисы общаются по внутренним Docker-именам (`http://product-service:5001`)
- Изоляция от внешней сети (только ApiGateway доступен снаружи)
- DNS-резолвинг работает "из коробки"

---

## 🧪 Тестирование

### Проверка работы сервисов

```bash
# Проверить health endpoint
curl http://localhost:5001/health

# Получить список товаров через API Gateway
curl http://localhost:5000/api/products

# Создать тестовый товар
curl -X POST http://localhost:5000/api/products \
  -H "Content-Type: application/json" \
  -d '{"name": "Test Product", "price": 99.99, "description": "Test"}'
```

### Просмотр логов

```bash
# Логи всех сервисов в реальном времени
docker-compose logs -f

# Логи только ProductService
docker-compose logs -f product-service
```

---

## 💡 Особенности реализации

### In-Memory хранилище

Для упрощения демонстрации все сервисы используют in-memory коллекции (`List<T>`) вместо реальной базы данных. Это позволяет:
- Быстро запустить проект без настройки БД
- Сфокусироваться на архитектуре микросервисов
- Легко тестировать CRUD операции

⚠️ **Важно:** Данные теряются при перезапуске контейнеров. Для продакшена нужно подключить PostgreSQL/SQL Server.

### API Gateway паттерн

ApiGateway реализует паттерн "Aggregator":
- Принимает все запросы от клиента
- Определяет, какому сервису направить запрос
- Возвращает ответ клиенту

Это упрощает фронтенд — ему нужно знать только один URL (Gateway).

### Graceful Shutdown

Все сервисы поддерживают корректное завершение работы:
- Обрабатывают сигнал `SIGTERM` от Docker
- Завершают активные запросы
- Освобождают ресурсы

---

## 📊 Мониторинг и логирование

### Логи

Docker собирает логи всех контейнеров в JSON формате:
- Размер файла: максимум 10MB
- Ротация: хранится максимум 3 файла
- Драйвер: `json-file`

Посмотреть логи:
```bash
docker-compose logs -f
```

### Health Monitoring

Каждые 30 секунд Docker проверяет состояние сервисов через `/health` endpoints. Если сервис не отвечает 3 раза подряд, он автоматически перезапускается.

---

## 🔒 Безопасность

В текущей версии (учебный проект) безопасность минимальна:
- Нет аутентификации/авторизации
- Нет HTTPS (только HTTP)
- Нет валидации входных данных

Для продакшена нужно добавить:
- JWT аутентификацию
- HTTPS/TLS шифрование
- Rate limiting
- Валидацию моделей
- CORS политику

---

## 🚧 Что можно улучшить (Roadmap)

- [ ] Подключить реальную базу данных (PostgreSQL/SQL Server)
- [ ] Добавить Entity Framework Core для работы с БД
- [ ] Реализовать аутентификацию через JWT
- [ ] Добавить RabbitMQ для асинхронной коммуникации между сервисами
- [ ] Настроить CI/CD pipeline (GitHub Actions)
- [ ] Добавить распределенный трейсинг (OpenTelemetry/Jaeger)
- [ ] Настроить мониторинг (Prometheus + Grafana)
- [ ] Мигрировать на Kubernetes для продакшн-оркестрации
- [ ] Добавить Swagger/OpenAPI документацию
- [ ] Реализовать кэширование через Redis

---

## 🤝 Вклад в проект

Если у вас есть идеи по улучшению или вы нашли баг — создавайте Issue или Pull Request!

---

## 📝 Лицензия

Проект создан в рамках курсовой работы и доступен для образовательных целей.

---

5. **Сравнение с монолитом:** Подготовь ответ на вопрос "Почему микросервисы, а не монолит?". Основные аргументы: независимое масштабирование, изоляция отказов, возможность использовать разные технологии для разных сервисов, упрощение деплоя.

Удачи с защитой курсовой! Если нужна помощь с Kubernetes манифестами или другими вопросами — обращайся.
