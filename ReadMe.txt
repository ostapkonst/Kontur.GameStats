������ ���������: 1.0.0-alpha3
�����: ������������ �. �.
�����: ostap.konstantinov@gmail.com
������� ������: http://dropmefiles.com/17YUl
������������: http://bit.ly/2km1XJj

������ ������� �������������� �������� Kontur.GameStats.Server.exe �� ����� �������. ����������� ������������ � ����� Logs ������� ����: <����>-<�����>.txt. �������� ���������� ������� �������� ���������� � �������������� ���� ������ SQLite � ������ GameStats.db. ��������� ��������� ����������� ����������� �������������� ����� appsettings.json.

�������������� �����:
	-? | -h | --help � �������� ������� "������"
	-p | --prefix <prefix> � ��������� http/https �������� (�� ���������: http://localhost:5000)
	
������������� � ���� ������ RESTful API:
	/servers/<endpoint>/info PUT, GET
	/servers/<endpoint>/matches/<timestamp> PUT, GET

	/servers/info GET

	/servers/<endpoint>/stats GET
	/players/<name>/stats GET

	/reports/recent-matches[/<count>] GET
	/reports/best-players[/<count>] GET
	/reports/popular-servers[/<count>] GET

������ �������������� � Visual Studio 2015 SP 3 ��� .NET Core 1.0.1. ������������� Microsoft .NET Core 1.0.1 - VS 2015 Tooling Preview 2. ���� ���������� ��������� � �������������� � ������� ������������ � �������������, ��-�� ������������ ���� �������, �������� ������������� ������ �� ������������. ���� � ���, ��� ��� ������ ������� ��������: "MSBuild.exe Kontur.GameStats.sln /p:Configuration=Release", ����� � *.exe �� ��������� ������������. �� ���� ������� �������� �������� ��������� ������, ��� ��������� ���� ������������ � Release-����������. �� ������ ������ ������ �������������� ����������� ���������� ����� BuildAndPublish.bat. � ���������� � �������� Release ����� ���������� Standalone(Selfhosting)-����������, ����������� ��������� RESTful API.

��� ���������� �������������� ����� ������ ������������, ��� ��������� �� ������ ������.

��������� ������������� ��� ����������� ������������ ����������� �������� "��� ������" � 2017 ����.
� ���������, ������������ �. �.