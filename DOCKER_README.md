# Docker-контейнеризация микросервисной архитектуры

## Структура проекта

```
/workspace
├── ApiGateway/          # API Gateway - точка входа
├── Frontend/            # Blazor WebAssembly приложение
├── UserService/         # Микросервис пользователей
├── ProductService/      # Микросервис товаров
├── OrderService/        # Микросервис заказов
├── docker-compose.yml   # Оркестрация контейнеров
└── README.md
```

## Запуск проекта

### Сборка и запуск всех сервисов

```bash
docker-compose up --build
```

### Запуск в фоновом режиме

```bash
docker-compose up -d --build
```

### Остановка сервисов

```bash
docker-compose down
```

### Остановка с удалением томов

```bash
docker-compose down -v
```

## Архитектура

### Сеть
Все сервисы объединены в единую сеть `microservices-network` типа bridge.

### Порты
- **Frontend**: 80 (веб-интерфейс)
- **ApiGateway**: 5000:8080 (точка входа API)
- **UserService**: 8080 (внутренний)
- **ProductService**: 8080 (внутренний)
- **OrderService**: 8080 (внутренний)

### Health Checks
Каждый сервис имеет endpoint `/health` для проверки работоспособности.

### Логирование
- Driver: `json-file`
- Максимальный размер файла лога: 10MB
- Максимальное количество файлов: 3

### Отказоустойчивость
- **Restart Policy**: `unless-stopped` - автоматический перезапуск при падении
- **Health Checks**: проверка каждые 30 секунд
- **Репликация**: 2 экземпляра для каждого сервиса (кроме frontend)
- **Graceful Shutdown**: поддерживается через SIGTERM

### Зависимости
- ApiGateway зависит от UserService, ProductService, OrderService
- Frontend зависит от ApiGateway

### Тома
- `userservice-data` - данные сервиса пользователей
- `productservice-data` - данные сервиса товаров
- `orderservice-data` - данные сервиса заказов

## Просмотр логов

### Логи всех сервисов

```bash
docker-compose logs
```

### Логи конкретного сервиса

```bash
docker-compose logs apigateway
docker-compose logs frontend
docker-compose logs userservice
```

### Логи в реальном времени

```bash
docker-compose logs -f
```

## Масштабирование

Для изменения количества реплик отредактируйте `docker-compose.yml`:

```yaml
deploy:
  replicas: 3  # Измените число на нужное количество
```

Или используйте команду:

```bash
docker-compose up -d --scale apigateway=3
```

## Проверка здоровья сервисов

```bash
curl http://localhost:5000/health
```

## Доступ к приложению

После запуска откройте в браузере:
- **Frontend**: http://localhost
- **API Gateway**: http://localhost:5000
