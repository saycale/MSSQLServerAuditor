<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Коэффициент попадания в кэш буфера" id="InstanceMemoryBufferCacheHitRatio.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Коэффициент попадания в кэш буфера" id="InstanceMemoryBufferCacheHitRatio.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/MSSQLResults">

			<html>
			<head>
				<link rel="stylesheet" href="$JS_FOLDER$/tablesorter/css/theme.mssqlserverauditor.css" type="text/css"/>

				<script src="$JS_FOLDER$/json-js/json2.js"></script>
				<script src="$JS_FOLDER$/jquery-1.11.1.js"></script>
				<script src="$JS_FOLDER$/tablesorter/js/jquery.tablesorter.js"></script>
				<script src="$JS_FOLDER$/tablesorter/js/jquery.tablesorter.widgets.js"></script>

				<script type="text/javascript">
					$(document).ready(function()
						{
							$("#myErrorTable").tablesorter({
								theme : 'MSSQLServerAuditorError',

								widgets: [ "zebra", "resizable" ],

								widgetOptions : {
									zebra : ["even", "odd"]
								}
							});

							$("#myTable").tablesorter({
								theme : 'MSSQLServerAuditor',

								widgets: [ "zebra", "resizable" ],

								widgetOptions : {
									zebra : ["even", "odd"]
								}
							});

							$("#myTable2").tablesorter({
								theme : 'MSSQLServerAuditor',

								widgets: [ "zebra", "resizable" ],

								widgetOptions : {
									zebra : ["even", "odd"]
								}
							});
						}
					);
				</script>
			</head>
			<body>
				<style>
					body { overflow: auto; padding:0; margin:0; }
				</style>
				<xsl:if test="MSSQLResult[@SqlErrorNumber!='0' or @SqlErrorCode!='']/child::node()">
				<table id="myErrorTable">
				<thead>
					<tr>
						<th>
							Зкземпляр
						</th>
						<th>
							Запрос
						</th>
						<th>
							Категория
						</th>
						<th>
							Наборов
						</th>
						<th>
							#
						</th>
						<th>
							Код
						</th>
						<th>
							Номер
						</th>
						<th>
							Сообщение
						</th>
					</tr>
				</thead>
				<tbody>
					<xsl:for-each select="MSSQLResult[@SqlErrorNumber!='0' or @SqlErrorCode!='']">
					<tr>
						<td>
							<xsl:choose>
								<xsl:when test="@instance != ''">
									<xsl:value-of select="@instance"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="@name != ''">
									<xsl:value-of select="@name"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="@hierarchy != ''">
									<xsl:value-of select="@hierarchy"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="@RecordSets != ''">
									<xsl:value-of select="@RecordSets"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="@RowCount != ''">
									<xsl:value-of select="@RowCount"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="@SqlErrorCode != ''">
									<xsl:value-of select="@SqlErrorCode"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="@SqlErrorNumber != ''">
									<xsl:value-of select="@SqlErrorNumber"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="SqlErrorMessage != ''">
									<xsl:value-of select="SqlErrorMessage"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
					</tr>
					</xsl:for-each>
				</tbody>
				</table>
				</xsl:if>
				<xsl:if test="MSSQLResult[@name='GetInstanceMemoryBufferCacheHitRatio' and @SqlErrorNumber='0' and @hierarchy='']/RecordSet[@id='1']/child::node()">
				<table id="myTable">
				<thead>
					<tr>
						<th>
							Экземпляр
						</th>
						<th>
							Время
						</th>
						<th>
							Коэффициент попадания в кэш буфера
						</th>
						<th>
							Коэффициент попадания в кэш буфера (базовое значение)
						</th>
						<th>
							Коэффициент попадания в кэш буфера (%)
						</th>
					</tr>
				</thead>
				<tbody>
					<xsl:for-each select="MSSQLResult[@name='GetInstanceMemoryBufferCacheHitRatio' and @SqlErrorNumber='0' and @hierarchy='']/RecordSet[@id='1']/Row">
					<tr>
						<td>
							<xsl:choose>
								<xsl:when test="../../@instance != ''">
									<xsl:value-of select="../../@instance"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="EventTime != ''">
									<xsl:value-of select="ms:format-date(EventTime, 'dd.MM.yyyy')"/>
									<xsl:text>&#160;</xsl:text>
									<xsl:value-of select="ms:format-time(EventTime, 'HH:mm:ss')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="BufferCacheHitRatio != ''">
									<xsl:value-of select="format-number(BufferCacheHitRatio, '###,###')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="BufferCacheHitRatioBase != ''">
									<xsl:value-of select="format-number(BufferCacheHitRatioBase, '###,###')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="BufferCacheHitRatioPercent != ''">
									<xsl:value-of select="format-number(BufferCacheHitRatioPercent, '###,##0.00')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
					</tr>
					</xsl:for-each>
				</tbody>
				</table>
				</xsl:if>

				<p></p>

				<xsl:if test="MSSQLResult[@name='GetInstancePageLifeExpectancy' and @SqlErrorNumber='0' and @hierarchy='']/RecordSet[@id='1']/child::node()">
				<table id="myTable2">
				<thead>
					<tr>
						<th>
							Экземпляр
						</th>
						<th>
							Время
						</th>
						<th>
							Продолжительность жизни страницы (секунд)
						</th>
					</tr>
				</thead>
				<tbody>
					<xsl:for-each select="MSSQLResult[@name='GetInstancePageLifeExpectancy' and @SqlErrorNumber='0' and @hierarchy='']/RecordSet[@id='1']/Row">
					<tr>
						<td>
							<xsl:choose>
								<xsl:when test="../../@instance != ''">
									<xsl:value-of select="../../@instance"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="EventTime != ''">
									<xsl:value-of select="ms:format-date(EventTime, 'dd.MM.yyyy')"/>
									<xsl:text>&#160;</xsl:text>
									<xsl:value-of select="ms:format-time(EventTime, 'HH:mm:ss')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="PageLifeExpectancy != ''">
									<xsl:value-of select="format-number(PageLifeExpectancy, '###,##0')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
					</tr>
					</xsl:for-each>
				</tbody>
				</table>
				</xsl:if>
			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>

	<mssqlauditorpreprocessors name="Коэффициент попадания в кэш буфера (описание)" id="InstanceMemoryBufferCacheHitRatio.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Коэффициент попадания в кэш буфера (описание)" id="InstanceMemoryBufferCacheHitRatio.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Коэффициент попадания в кэш буфера</h1>

				<p> Чтобы узнать, не требуется ли SQL Server больше памяти, взгляните на
				Коэффициент попадания в кэш буфера и Время жизни страницы. Вот, что
				говорится о Коэффициенте попадания в кэш буфера в «Books On Line»:</p>

				<p><em>"Коэффициент попадания в кэш буфера сообщает процент страниц,
				найденных в кэше буфера без необходимости чтения с диска. Данный коэффициент
				показывает общее количество попаданий к кэш, деленное на общее количество
				обращений в кэш за последние несколько тысяч доступов к странице. Данный
				коэффициент со временем изменяется незначительно. Поскольку чтение из КЭШе
				требует меньшего использования памяти, нежели чтение с диска, для Вас лучше,
				чтобы данный коэффициент был высоким. Как правило, вы можете увеличить
				коэффициент попадания в кэш буфера за счет увеличения объема памяти,
				доступной для SQL Server."</em></p>

				<p><em>"Это объём страниц памяти, в котором читаются данные страницы. Важным
				показателем производительности кэша буфера является процент попадания в кэш
				буфера. Он сообщает процент страниц с данными, найденных в кэше, а не на
				диске. Значение 95% означает, что страницы были найдены в памяти 95%
				времени. В оставшихся 5% необходим физический доступ к диску. Постоянное
				значение ниже 90% означает, что серверу необходимо больше физической памяти.
				</em></p>

				<p> По сути, он означает процент объема данных, которые SQL Server хранил в
				кэше без необходимости считывания с диска. Лучше, чтобы это число было как
				можно ближе к 100.</p>

				<p> Чтобы вычислить коэффициент попадания в кэш буфера, необходимо сделать
				запрос sys.dm_os_performance_counters в режиме динамического управления. Для
				выполнения расчета необходимо 2 счетчика, один счетчик – это коэффициент
				попадания в кэш буфера, а другой – база коэффициента попадания в кэш буфера.
				База коэффициента попадания в кэш буфера делится на коэффициент попадания в
				кэш, что дает нам Коэффициент попадания в кэш буфера. Вот запрос, который
				нужно выполнить. Данный запрос будет работать только на SQL Server версии
				2005 и выше.</p>

				<h1 class="firstHeading">Время жизни страницы</h1>

				<p> Теперь рассмотрим параметр "Продолжительность жизни страницы".
				Продолжительность жизни страницы - количество секунд, в течение которых
				страница находится в буфере. В идеале она должна быть выше 300 секунд. Если
				она составляет менее 300 секунд, это может означать вытеснение памяти,
				очистку кэша или отсутствующие индексы.</p>

				<p><a href="http://blogs.lessthandot.com/index.php/DataMgmt/DBAdmin/MSSQLServerAdmin/use-sys-dm_os_performance_counters-to-ge" target="_blank">Use sys.dm_os_performance_counters to get your Buffer cache hit ratio and Page life expectancy counters</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>
