Meanwhile :
https://blogs.taiga.nl/martijn/2017/11/24/building-and-asp-net-core-mvc-app-with-npm-and-webpack-asp-net-core-2-0-edition/      the basis
https://codeburst.io/how-to-use-webpack-in-asp-net-core-projects-a-basic-react-template-sample-25a3681a5fc2
provide guidance

Right now :
want to get Bootstrap/jquery/its two siblings/popper.js/toottip.js integrated into webPack so it generates the dist files
I have jQuery integrated with a manual build ('npm run wbp') and have a plugin to automatically load jQuery into the js bundle, 
I have Bootstrap installed and loading as a module, and its styling the nav bar and menu! I had to update this stuff for BootStrap 4 
I have site.css loading as a module, however I do not have it doing an automatic build based on the source file save

****not sure if I'm loading popper.js and tooltip.js - better go over that and make sure


****Then I want to get changes to source stuff to do an automatic build - specifically site.css, the js source.

Onward and Upward
get your head around this webpack production/development bundle thingy

get your head around the 'extract text' and 'uglify' thingys in sample-25a3681a5fc2 .  The former particularly will get the CSS unto a separate file rather than inline

do the funky hot module replacement, also in sample-25a3681a5fc2, which will get you into Core WebpackDevMiddleware

switch to typescript 3.0 as your language of choice.  That means you have to integrate transpilation into js and also HMR of the js to the browser

Add React to your projects-a-basic-react-template-sample-25a3681a5fc2

Do the whole thing again, based on Core 2.1, with guidance from https://blogs.taiga.nl/martijn/2018/06/14/lean-asp-net-core-2-1-manually-setup-a-razor-pages-project-with-bootstrap-npm-and-webpack/ 

Figure out the various ways ( if any! ) React plays with Razor and optimise the HTML accordingly

Then have a look at the overall project component structure - after all thats where you want to get to

Write a React component, such as a 'tag helper' , maybe even a calendar component.  
Here's a primer https://blog.flowandform.agency/create-a-custom-calendar-in-react-3df1bfd0b728



