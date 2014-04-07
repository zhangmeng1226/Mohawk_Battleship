Imports MBC.Shared
Imports MBC.Shared.Attributes

Namespace MBC
    Namespace Controllers

        ' This is a controller that uses a pseudo-random number generator to make all of its decisions. This
        ' controller is highly documented and gives a good idea of how to develop a controller for use in MBC.
        ' Every controller must implement the IBattleshipController interface from the shared framework in
        ' order to be detected by the MBC core.

        ' Then, each controller must use at least three attributes to describe itself, which are the NameAttribute
        ' VersionAttribute, and CapabilitiesAttribute. They are simple to use; look at the attributes set to the RandomBot below to
        ' get an idea of how to set attributes. You can see which attributes are available by looking in the
        ' "Controller Plugin" project and opening the "Attributes" folder. Note that you do not need to
        ' type out the word "Attribute" after the attribute you wish to use.

        ' Note that each controller has a time limit before they lose the round, by default, this limit is 200ms.
        ' If the time limit is exceeded, the bot will lose the round, and the next round will begin immediately.

        ' In this framework, two bots are pitted against each other in a match format of many rounds. By default
        ' both bots will play each other in 1,000 rounds of battleship. Your bot's memory will persist between rounds
        ' allowing your bot to do long term analysis throughout the remainder of the match.

        ' This file is intended to act as a demonstration of how to create a battleship bot, feel free to copy and paste
        ' this file, refactor every occurrence of RandomBot to your own bot name, and use this file as your starting point.

        ' Bellow is a series of attributes that describes the bot name, capabilities, and authors.
        ' Name: The name of your battleship bot
        ' Capabilities: The game modes that your bot is capable of playing. Initially this will only be classic.
        ' Description: A quick overview of the bot for your own reference. optional
        ' Author: The name of the programmer that authored this bot. optional
        ' AcademicInfo: This is intended for students, the first string is the academic institute, the second string is the program of study, and the int is the year that student is in.
        <Name("RandomBot VB Old")>
        <Version(1, 0)>
        <Capabilities(GameMode.Classic)>
        <Description("A controller that uses a random number generator to make all of its decisions.")>
        <Author(FirstName:="Ryan", LastName:="Albert", AKAName:="piknik",
                Biography:="I assisted in the development of the framework =]"
                )>
        <AcademicInfo("Mohawk College", "Software Development", 2)>
        Public Class RandomBot_old
            Inherits Controller

            ' This object provides a pseudo random number generator for use in this bot.
            Private rand As Random

            ' This is a list of shots that this controller has against another controller or controllers.
            ' It will start out being filled with every possible shot made.
            Private shotQueue As ShotList

            ' This method is called each time the bot begins a new round. This is a good place to initialize
            ' objects that are specific to this game, such as your map and random number generator.
            Public Overrides Sub NewRound()
                SetShots()
                rand = Match.Random
            End Sub

            ' This method is called when the controller is required to place ships. It is given a collection
            ' of Ship objects that can be accessed.
            ' <param name="ShipList">A collection of Ship objects to place.</param>
            Public Overrides Function PlaceShips(initialShips As ShipList) As ShipList

                ' First we'll refer to the ships given to us through a single variable.
                Dim myShips = initialShips

                'This loop will continue until all of the Ship objects have been placed.
                ' NOTE: myShips.ShipsPlaced returns false if all ships have not been placed in a valid location.
                While Not myShips.ShipsPlaced
                    Dim randomCoords = RandomCoordinates()

                    Dim orientation = RandomShipOrientation()

                    ' Use the function within the ShipList object "myShips" to place a ship for the controller.
                    ' As explained in the PlaceShip() method of the ShipList, placing a ship at the randomly
                    ' generated coordinates may fail, which is why we loop until we find a valid placement.
                    myShips.PlaceShip(randomCoords, orientation, Match.FieldSize)

                End While

                ' After all ships has been placed, return myShips back to the framework.
                Return myShips
            End Function

            ' This method is called when a shot is available to the controller. The Shot object is a reference
            ' to a copy held by the competition and is expected to be modified to the desired target. By default,
            ' the Shot receiver is the next controller in the turn.

            ' <param name="shot">The Shot to make.</param>
            Public Overrides Function MakeShot() As Shot
                Return NextRandomShot()
            End Function

            ' This method is called when this controller won the round.
            Public Overrides Sub RoundWon()
                SendMessage("Yay, I won! What are the chances of that?")
            End Sub

            ' This method is called when this controller lost the round.
            Public Overrides Sub RoundLost()
                SendMessage("Unsurprisingly I lost...")
            End Sub

            ' This method is called when this controller begins a new match.
            ' You can use this method for any bot initialization at the beginning of a match.
            ' This method is optional and does not need to be implemented.
            Public Overrides Sub NewMatch()

            End Sub

            ' This method is called when this controller finishes a match.
            ' You can use this method for any debugging purposes you may need at the end of a match.
            ' such as writing out variables to a file or to the console.
            ' this method is optional and does not need to be implemented.
            Public Overrides Sub MatchOver()

            End Sub

            ' This method is called when your bot scores a hit against an enemy ship.
            ' The shot variable contains the co-ordinates of the shot, and sunk boolean will be
            ' true or false if this shot caused a ship to sink. This method is useful for mapping operations
            Public Overrides Sub ShotHit(shot As Shot, sunk As Boolean)

            End Sub

            ' This method is called when your bot scores a miss. This method is useful for mapping operations.
            ' The shot variable contains the coordinates of the miss.
            Public Overrides Sub ShotMiss(shot As Shot)

            End Sub

            ' This method is called each time an opponent bot fires a shot. This method
            ' allows your bot the opportunity to track the shots of your opponent for your own
            ' analysis.
            Public Overrides Sub OpponentShot(shot As Shot)

            End Sub

            ' This method generates a random set of coordinates within the match field boundaries.
            ' NOTE: This is a helper method for the random bot and is not required, it here for demonstration
            ' purposes only.
            Private Function RandomCoordinates() As Coordinates
                Dim xCoord = rand.Next(Match.FieldSize.X)

                Dim yCoord = rand.Next(Match.FieldSize.Y)

                Return New Coordinates(xCoord, yCoord)
            End Function

            ' This method randomly returns one of two ShipOrientation enums.
            ' NOTE: This is a helper method for the random bot and is not required, it here for demonstration
            ' purposes only.
            Private Function RandomShipOrientation() As ShipOrientation
                Dim orientations = {ShipOrientation.Horizontal, ShipOrientation.Vertical}

                Return orientations(rand.Next(2))
            End Function

            ' This method fires off the next shot in our queue of shots.
            ' NOTE: This is a helper method for the random bot and is not required, it here for demonstration
            ' purposes only.
            Private Function NextRandomShot() As Shot
                Dim randomShotIndex = rand.Next(shotQueue.Count)

                Dim randomShot = shotQueue(randomShotIndex)

                shotQueue.Remove(randomShot)

                Return randomShot
            End Function

            ' This method creates a queue of shots to be made throughout the game for the random bot.
            ' NOTE: This is a helper method for the random bot and is not required, it here for demonstration
            ' purposes only.
            Private Sub SetShots()
                shotQueue = New ShotList()

                shotQueue.MakeReceivers(AllOpponents())

                shotQueue.Invert(Match.FieldSize)
            End Sub

        End Class
    End Namespace
End Namespace