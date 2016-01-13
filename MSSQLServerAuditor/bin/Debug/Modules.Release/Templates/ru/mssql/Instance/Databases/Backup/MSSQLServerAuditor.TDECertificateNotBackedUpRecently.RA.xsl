<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Базы данных без архива для TDE сертификатов" id="TDECertificateNotBackedUpRecently.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Базы данных без архива для TDE сертификатов" id="TDECertificateNotBackedUpRecently.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">TDE Certificate Not Backed Up Recently</h1>

				<p>The certificate is used to encrypt database, but there certificate was
				not backuped up recently or backup is not up to date.</p>

				<p>SQL Server 2008 introduced Transparent Data Encryption – a
				set-it-and-forget-it way to keep your databases protected on disk. If
				someone steals your backup tapes or your hard drives, they’ll have a tougher
				time getting access to the data. Just set it up and you’re done.</p>

				<p>However, if you don’t back up your encryption certificate and password,
				you’ll never be able to restore those databases!</p>

				<p>This report checks sys.databases looking for databases that have been
				encrypted, and also checks to make sure that the certificates have been
				backed up recently.</p>

				<p><strong>Как это исправить:</strong> Выполните архивное копирование для
				сертификатов.</p>

				<p><strong>Уровень сложности:</strong> Легкий</p>

				<p><strong>Уровень опасности:</strong> Средний</p>

				<p><a href="http://BrentOzar.com/go/tde" target="_blank">TDE Certificate Not Backed Up Recently</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>
