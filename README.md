![TeamCity Monitor  screen dump][screendump]

# TeamCity Monitor

## Overview

This is a tool for displaying an overview of the build statuses of the projects in a TeamCity build server.

Features

* The display groups the build configurations for a project together to make the overview clearer.

* It uses 3 colors:

	* Green - the last build for the **default VCS branch** was successful
	* Orange - the build configuration is currently being built by TeamCity (**any VCS branch**)
	* Red - the last build for the **default VCS branch** was successful

* Each box uses CSS opacity to decrease the color intensity of old builds so it's easy to separate new failed
  builds from old ones. 

* The display is automatically refreshed with an interval that is much shorter than Teamcity's own. By default
  it refreshes every 5 seconds. This is possibly since it shows less detailed information than TeamCity's
  "Projects" view.

* The last refresh time is shown in the bottom left corner of the display so that failed updates
  are easily detected. A red error box is also displayed if there is a refresh failure.

The underlying data source is TeamCity's CCTray-compatibly data feed which is not very
documented, but it's mentioned here https://confluence.jetbrains.com/display/TCD9/REST+API#RESTAPI-CCTray

This feed is quick and provides most of the information that's interesting to show, so it was selected as the
source to use for this tool, rather than the full Teamcity API which is very complete and powerful, but slow
and a bit complicated to use.

## Usage

To use this tool, update the following `AppSettings` in `Web.config`:

```
<!-- TODO Fill in these: -->
<add key="TeamCityHost" value="[http://your-teamcity-server]" />
<add key="TeamCityUser" value="[teamcity account]" />
<add key="TeamCityPassword" value="[teamcity password]" />
```

Then build the project in Visual Studio 2013 or 2015. Test it locally or deploy to
a shared location and you're done!

[screendump]: media/screendump.png