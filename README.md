Тестовое приложение для демонстрации работы UIAutomation. Приложение генерирует строку из случайных чисел, а затем воспроизводит заранее подготовленный процесс.

## Требования для запуска:
- Процесс был "записан" на системе Windows 10 Home с английским языком интерфейса. При проигрывании процесса на другой системе или с другим языком интерфейса потребуется сделать правки в файле *Program.cs* (определения шагов процесса).
- Поиск строки выполняется в браузере Chrome, перед запуском приложения он должен быть закрыт (во время воспроизведения процесса он будет запущен автоматически с определенными настройками).