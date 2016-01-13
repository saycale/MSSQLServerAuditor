<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Server Jobs" id="Jobs.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Server Jobs" id="Jobs.HTML.en" column="1" row="1" colspan="1" rowspan="1">
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
							Instance
						</th>
						<th>
							Query
						</th>
						<th>
							Hierarchy
						</th>
						<th>
							RecordSets
						</th>
						<th>
							#
						</th>
						<th>
							Code
						</th>
						<th>
							Number
						</th>
						<th>
							Message
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
				<xsl:if test="MSSQLResult[@name='GetJobsList' and @SqlErrorNumber='0' and @hierarchy='']/RecordSet[@id='1']/child::node()">
				<table id="myTable">
				<thead>
					<tr>
						<th rowspan="2">
							Instance
						</th>
						<th rowspan="2">
							JobName
						</th>
						<th rowspan="2">
							JobOwner
						</th>
						<th rowspan="2">
							JobEnabled
						</th>
						<th colspan="5">
							JobSchedule
						</th>
						<th rowspan="2">
							JobCreated
						</th>
						<th rowspan="2">
							JobChanged
						</th>
					</tr>
					<tr>
						<th>
							Name
						</th>
						<th>
							Active
						</th>
						<th>
							Start
						</th>
						<th>
							End
						</th>
						<th>
							Description
						</th>
					</tr>
				</thead>
				<tbody>
					<xsl:for-each select="MSSQLResult[@name='GetJobsList' and @SqlErrorNumber='0' and @hierarchy='']/RecordSet[@id='1']/Row">
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
						<td>
							<xsl:choose>
								<xsl:when test="JobName != ''">
									<xsl:value-of select="JobName"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:if test="IsJobOwnerIssue != 0">
								<xsl:attribute name="style">font-weight: bold; color: blue;</xsl:attribute>
							</xsl:if>
							<xsl:choose>
								<xsl:when test="JobOwner != ''">
									<xsl:value-of select="JobOwner"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="JobEnabled != ''">
									<xsl:value-of select="JobEnabled"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="JobScheduleName != ''">
									<xsl:value-of select="JobScheduleName"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="JobScheduleEnabled != ''">
									<xsl:value-of select="JobScheduleEnabled"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="JobScheduleActiveStartDateTime != ''">
									<xsl:value-of select="ms:format-date(JobScheduleActiveStartDateTime, 'dd/MM/yyyy')"/>
									<xsl:text>&#160;</xsl:text>
									<xsl:value-of select="ms:format-time(JobScheduleActiveStartDateTime, 'HH:mm:ss')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="JobScheduleActiveEndDateTime != ''">
									<xsl:value-of select="ms:format-date(JobScheduleActiveEndDateTime, 'dd/MM/yyyy')"/>
									<xsl:text>&#160;</xsl:text>
									<xsl:value-of select="ms:format-time(JobScheduleActiveEndDateTime, 'HH:mm:ss')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="JobScheduleDescription != ''">
									<xsl:value-of select="JobScheduleDescription"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="JobDateCreated != ''">
									<xsl:value-of select="ms:format-date(JobDateCreated, 'dd/MM/yyyy')"/>
									<xsl:text>&#160;</xsl:text>
									<xsl:value-of select="ms:format-time(JobDateCreated, 'HH:mm:ss')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="JobDateModified != ''">
									<xsl:value-of select="ms:format-date(JobDateModified, 'dd/MM/yyyy')"/>
									<xsl:text>&#160;</xsl:text>
									<xsl:value-of select="ms:format-time(JobDateModified, 'HH:mm:ss')"/>
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

	<mssqlauditorpreprocessors name="Description" id="ServerJobs.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Description" id="ServerJobs.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Jobs Owned by User Accounts</h1>

				<p>SQL Server Agent jobs are like hot potatoes: they're owned by whoever
				touched them last. The job owner doesn't really mean much as jobs can be
				owned by anyone and they'll still work the same way.</p>

				<p>Right up until the job owner's account is dropped.</p>

				<p>If the SQL Server was managed by someone who's no longer with the
				company, all their jobs will suddenly stop working when SQL Server can't
				verify the job owner's account. (This can also pop up if there's an Active
				Directory problem when the job needs to run.) The fix is to have the
				built-in SA account own all the jobs.</p>

				<p>This part of report lists jobs owned by accounts other than SA. If you've
				changed the name of the SA account, the jobs will show up here, but don't
				pat yourself on the back just yet. Changing the name of the SA account can
				break some SQL Server service packs, so make sure you understand the risks
				there.</p>

				<p><strong>How to correct it:</strong> If the jobs have been failing because
				they're owned by a user whose account has since been disabled, you probably
				don't want to fix this problem right away. The jobs may have been failing
				for months, and when you enable them again, you may run into serious side
				effects. For example, we've seen cases where developers didn't know why the
				job was failing, and they built a separate process to do the same work. When
				the jobs were enabled again, the business processes broke.</p>

				<p>You also want to understand the contents of any job before you enable it
				again. However, if the job's still running on a regular basis, then it makes
				sense to change that job owner to SA to make sure it continues to run
				if/when the job owner's account is disabled.</p>

				<p>When you're ready to fix the problem, you can simply right-click on each
				job in SSMS and click Properties. Change the owner to SA and click OK.</p>

				<p><strong>Level of difficulty:</strong> Easy</p>

				<p><strong>Level of severity:</strong> Easy</p>

				<p><a href="http://www.brentozar.com/blitz/jobs-owned-by-user-accounts/" target="_blank">Blitz Result: Jobs Owned by User Accounts</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

