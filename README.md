# Home Temperature Monitor

Stack: .Net 6, Akka.Net, Arduino, GRPC, Blazor, EF Core

Project has two main applications: 
- HTM console app
- HTM.Web blazor server side app

HTM communicates with arduino through the serial port and asks for the current temperature repeatedly. The result is saved in the ms sql database.
With HTM.Web we can view the temperatures history on a line chart and diagnose arduino board.
Both applications connect to each other using the grpc protocol.


![1](https://user-images.githubusercontent.com/42341432/163802425-bb19bd28-7f94-4be8-b51a-8169f14e7620.png)
