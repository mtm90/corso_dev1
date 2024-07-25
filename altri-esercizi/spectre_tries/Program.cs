// Create the layout
using Spectre.Console;

var layout = new Layout("Poker room")
    .SplitColumns(
        new Layout("Left"),
        new Layout("Right")
            .SplitRows(
                new Layout("Your Hand"),
                new Layout("Computer Hand")));

// Update the left column
layout["Left"].Update(
    new Panel(
        Align.Center(
            new Markup("Hello [blue]World![/]"),
            VerticalAlignment.Middle))
        .Expand());

layout["Left"].Size(100);


layout["Your Hand"].Update(new Panel(
        Align.Center(
            new Markup("Cards"),
            VerticalAlignment.Middle))
        .Expand()
);
layout["Computer Hand"].Update(new Panel(
        Align.Center(
            new Markup("Cards"),
            VerticalAlignment.Middle))
        .Expand()
);



// Render the layout
AnsiConsole.Write(layout);