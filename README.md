# Home Temperature Monitor

Stack: .Net 6, Akka.Net, Arduino, GRPC, Blazor, EF Core

Project has two main applications: 
- HTM console app
- HTM.Web blazor server side app

HTM communicates with arduino through the serial port and asks for the current temperature repeatedly. The result is saved in the ms sql database.
With HTM.Web we can view the temperatures history on a line chart and diagnose arduino board.
Both applications connect to each other using the grpc protocol.
