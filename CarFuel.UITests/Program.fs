//these are similar to C# using statements
open canopy
open runner
open System

////start an instance of the firefox browser
////start firefox
//chromeDir <- "C:\\Driver"
//start chrome
//
////this is how you define a test
//"taking canopy for a spin" &&& fun _ ->
//    //this is an F# function body, it's whitespace enforced
//
//    //go to url
//    url "http://lefthandedgoat.github.io/canopy/testpages/"
//
//    //assert that the element with an id of 'welcome' has
//    //the text 'Welcome'
//    "#welcome" == "Welcome"
//
//    //assert that the element with an id of 'firstName' has the value 'John'
//    "#firstName" == "John"
//
//    //change the value of element with
//    //an id of 'firstName' to 'Something Else'
//    "#firstName" << "Something Else"
//
//    //verify another element's value, click a button,
//    //verify the element is updated
//    "#button_clicked" == "button not clicked"
//    click "#button"
//    "#button_clicked" == "button clicked"


let baseUrl = "http://localhost:2366" 
let userEmail = "user" + DateTime.Now.Ticks.ToString() + "@company.com"
let pwd = "Test999/*"
chromeDir <- "C:\\Driver" 
start chrome 

"Anonumous Users cannot add a new car" &&& fun _ ->
    url (baseUrl + "/Cars")
    notDisplayed "a[href='/Cars/Add']"//Tag a that has attribute with this value

"Sign Up" &&& fun _ ->
    url (baseUrl + "/Account/Register")
    "#Email" << userEmail
    "#Password" << pwd
    "#ConfirmPassword" << pwd
    click "input[type=submit]"
    on baseUrl

"Log in" &&& fun _ ->
    url (baseUrl + "/Account/Login")
    "#Email" << userEmail
    "#Password" << pwd
    click "input[type=submit]"
    on baseUrl

"Click add link then go to create page" &&& fun _ ->
    url (baseUrl + "/Cars")
    displayed "a[href='/Cars/Add']"
    click "a[href='/Cars/Add']"
    on (baseUrl + "/Cars/Add")

"User can add a car and see it in the list page" &&& fun _ ->
    url (baseUrl + "/Cars/Add")
    "input#Name" << "Car:" + userEmail
    click "button#singlebutton"
    on (baseUrl + "/Cars")
    "span#totalCars" == "1"
    "div.car-name" == "Car:" + userEmail


//run all tests
run()

printfn "press [enter] to exit"
System.Console.ReadLine() |> ignore

quit()