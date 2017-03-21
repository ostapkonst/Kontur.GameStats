Версия программы: 1.0.0-alpha3
Автор: Константинов О. В.
Почта: ostap.konstantinov@gmail.com
Готовая сборка: http://dropmefiles.com/17YUl
Документация: http://bit.ly/2km1XJj

Запуск сервера осуществляется командой Kontur.GameStats.Server.exe из папки проекта. Логирование производится в папку Logs файлами вида: <дата>-<номер>.txt. Хранение статистики игровых серверов происходит с использованием базы данных SQLite с именем GameStats.db. Настройка программы реализуется посредством редактирования файла appsettings.json.

Поддерживаемые ключи:
	-? | -h | --help — открытие раздела "помощь"
	-p | --prefix <prefix> — установка http/https префикса (по умолчанию: http://localhost:5000)
	
Реализованный в этой версии RESTful API:
	/servers/<endpoint>/info PUT, GET
	/servers/<endpoint>/matches/<timestamp> PUT, GET

	/servers/info GET

	/servers/<endpoint>/stats GET
	/players/<name>/stats GET

	/reports/recent-matches[/<count>] GET
	/reports/best-players[/<count>] GET
	/reports/popular-servers[/<count>] GET

Проект разрабатывался в Visual Studio 2015 SP 3 как .NET Core 1.0.1. Использовался Microsoft .NET Core 1.0.1 - VS 2015 Tooling Preview 2. Хоть разработка программы и осуществлялась в строгом соответствии с документацией, из-за особенностей типа решения, пришлось незначительно отойти от документации. Дело в том, что при сборке решения командой: "MSBuild.exe Kontur.GameStats.sln /p:Configuration=Release", папка с *.exe не содержала зависимостей. По этой причине пришлось изменить процедуру сборки, для включения всех зависимостей в Release-директорию. На данный момент сборка осуществляется посредством выполнения файла BuildAndPublish.bat. В результате в каталоге Release будет находиться Standalone(Selfhosting)-приложение, реализующий требуемый RESTful API.

При разработке использовалась более ранняя документация, чем доступная на момент релиза.

Программа предоставлена для внутреннего тестирования сотрудникам компании "СКБ Контур" в 2017 году.
С уважением, Константинов О. В.