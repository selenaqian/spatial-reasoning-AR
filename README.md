# spatial-reasoning-AR
Independent study project exploring the use of AR for spatial reasoning education.

Start date: 8/17/2020

Spatial reasoning is an area with one of the most persistent gender gaps in cognitive skills, and this skill is widely regarded as important in engineering and other STEM fields. It is also an area in which training can significantly improve skills in a short period of time and increase likelihood to stay in STEM fields. This is particularly important because there still exists a gender gap in STEM, a gap that starts to appear in middle school and only widens from there.

Education in the time of social distancing additionally creates a relatively novel situation — while online learning courses have existed for some time, the pandemic has necessitated that the majority of students move to online, remote learning rather than the minority. In this time, AR provides an opportunity to more closely simulate the in-person experience. It allows students to have physical interaction with virtual objects, potentially in some of the same or similar ways as in the classroom, as well as in some novel ways that would not be possible or feasible with physical objects.

This project focuses on diving into the affordances of augmented reality (AR) and its uses in pedagogy. I am building on my previous experience with a Bass Connections project focused on problem-based learning and spatial reasoning to improve middle-school aged girls’ confidence in math and STEM in order to lessen the gender gap in those fields.

Overall Structure:

Challenges:
1. AR Workflow - Building and testing the app multiple times in one coding session takes time and becomes quite tedious when making only minor changes. In order to limit this, I worked to create a distinct feature or two before building to get the most out of that build and debugging. I should have also looked into writing unit tests for this project — this is something I have only done in the past for Java projects.

1. Flexibility of Code - One of my main goals with this project was to create a flexible and extensible project that would allow for easy creation of new exercises. I believe I was fairly successful with this, through creating custom C# classes to hold the elements and methods necessary for a Question and a Category of questions. These serializable classes allow me to read in JSON text about the objects and create exercises based on that.

Assumptions:
* Data file formatted correctly
  * proper JSON
  * rotations within achievable ranges
  * model parameter matches an existing prefab

User Testing Process:

Testing with even a small sample size (5 subjects) revealed several important insights about the function of the app and of how people interact with AR.

I explained that the app was intended for spatial reasoning learning and used augmented reality. I presented each person with the image marker and the phone with the app's welcome screen already loaded, and I allowed them to explore from there.

Key takeaways:
* On-screen functions generally made sense - start button, rotations button, submit button, progress text in the corner.
* Users are unfamiliar with AR and how to navigate in that space - not immediately connecting the physical marker to changes in what is seen on the screen; indicates need for additional explanation in tutorial.
* Performance of the image tracking in the app needs improvement - lag time between movement of paper and movement of object causes disconnect, frustration, and even motion sickness in one subject.
* Current display of prompt object shown to match exists in a different plane and causes some confusion about what rotation is actually necessary - seeing model from a slightly higher view as opposed to the object resting on a table.
* When unsure what to do, response is often to go to the screen and click, hoping for some sort of response - indicates may be useful to include additional hint prompts triggered by clicking on the screen.
