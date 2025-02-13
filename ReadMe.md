# Cifcore Test Project

## **Описание проекта**
Этот проект демонстрирует реализацию системы управления сетевыми запросами, основанной на `RequestQueueManager`, для обработки данных, таких как прогнозы погоды и факты о породах собак. Проект построен с использованием **Unity 2022.3.37f**, **Zenject**, **Cysharp.Threading.Tasks (UniTask)**, **UniRx** и **Dotween**.

## **Запуск**
1. Откройте проект в Unity 2022.3.37f1 или более поздней версии.
2. Откройте сцену `Main` расположенную в `Assets/_App/Scenes/Main.unity`.
3. Нажмите `Play` для запуска проекта.
4. В классе RequestQueueManager можно включить симуляцию задержек для проверки отмены запросов в редакторе Unity. (см. раздел **Основные функции**).


### **Основные функции**
- **Обработка запросов через очередь**: Использование `RequestQueueManager` для управления последовательностью запросов.
- **Асинхронное выполнение**: Логика запросов и обработки данных реализована с использованием `UniTask` для высокоэффективного асинхронного выполнения.
- **Отмена операций**: Все операции поддерживают `CancellationToken` для контроля выполнения задач.
- **Симуляция сетевых задержек**:
        - Для включения задержек для проверки отмен запросов в редакторе Unity установите `RequestQueueManager._simulateDelayInEditor = true`.
        - Параметр `RequestQueueManager._requestDelay` задаёт продолжительность задержки (по умолчанию 0.1 секунды).
- **Работа с API**:
    - `FactProvider` — загрузка информации о породах собак.
    - `WeatherService` — получение прогноза погоды.

---

## **Структура проекта**

### **Ключевые файлы и папки**
- **`Runtime/Web`**: Содержит основной код для взаимодействия с API и управления запросами.
    - `RequestQueueManager.cs`: Управляет очередями запросов, выполняет их по порядку и обрабатывает результаты.
    - `FactProvider.cs`: Отвечает за загрузку данных о породах собак.
    - `WeatherService.cs`: Реализует функционал получения прогноза погоды.
- **`Runtime/UI`**: Управление пользовательским интерфейсом, отображение данных, таких как прогноз погоды или факты.
- **`DTO`**: Определение структур данных для сериализации/десериализации JSON ответов от API.

### **Основные классы**
1. **RequestQueueManager**
    - Управляет асинхронными запросами.
    - Обеспечивает добавление запросов с URL и обработчиком результата.
    - Поддерживает отмену запросов через `CancellationToken`.

2. **FactProvider**
    - Использует `RequestQueueManager` для загрузки списка пород собак и конкретной информации о породах.
    - Примеры методов:
        - `GetBreedsListAsync`: Загружает список пород собак.
        - `GetBreedByIdAsync`: Загружает информацию о конкретной породе.

3. **WeatherService**
    - Позволяет загружать прогноз погоды через API.
    - Метод `GetWeatherForecastAsync` возвращает список погодных данных.

4. **DTO**
    - `BreedsListResponse`: Ответ от API с данными о породах.
    - `BreedResponse`: Ответ от API с деталями о конкретной породе.
    - `WeatherForecast`: Модель прогноза погоды.

---
## Потенциальные улучшения
1. **RequestsQueueManager**:
    - Добавить поддержку других типов запросов (POST, PUT, DELETE).
    - Реализовать механизм приоритетов в очереди запросов.
    - Добавить механизм повторов запросов в случае ошибок.
    - Вынести симуляцию задержек в отдельный класс в custom editor tool.
2. **FactProvider и WeatherService**:
   - Избавиться от дублирования логики для обработки запросов через вынесение общей части в базовый класс.
3. **Работа с UI и ресурсами**
   - Загрузка экранов основана на прямых ссылках, что неоптимально для больших проектов. Нужно использовать систему управления экранами и их ресурсами, например с помощью Addressables.
4. **Добавление тестов**
   - Добавить модульные тесты для проверки логики работы с API и обработки данных.
