Imports MBC.Shared
Imports MBC.Shared.Attributes

Namespace MBC
    Namespace Controllers
        <Name("RandomBot VB")>
        <Version(1, 0)>
        <Capabilities(GameMode.Classic, GameMode.Salvo, GameMode.Multi)>
        <Description("A controller that uses a random number generator to make all of its decisions.")>
        <Author(FirstName:="Ryan", LastName:="Albert", AKAName:="piknik",
                Biography:="I assisted in the development of the framework =]"
                )>
        <AcademicInfo("Mohawk College", "Software Development", 2)>
        Public Class RandomBot
            Inherits Controller

            Private rand As Random

            Private shotQueue As ShotList

            Public Overrides Sub NewRound()
                SetShots()
                rand = New Random()
            End Sub

            Public Overrides Function PlaceShips(initialShips As ShipList) As ShipList
                Dim myShips = initialShips

                While Not myShips.ShipsPlaced
                    Dim randomCoords = RandomCoordinates()

                    Dim orientation = RandomShipOrientation()

                    myShips.PlaceShip(randomCoords, orientation, Register.Match.FieldSize)

                End While

                Return myShips
            End Function

            Public Overrides Function MakeShot() As Shot
                Return NextRandomShot()
            End Function

            Public Overrides Sub RoundWon()
                SendMessage("Yay, I won! What are the chances of that?")
            End Sub

            Public Overrides Sub RoundLost()
                SendMessage("Unsurprisingly I lost...")
            End Sub

            Private Function RandomCoordinates() As Coordinates
                Dim xCoord = rand.Next(Register.Match.FieldSize.X + 1)

                Dim yCoord = rand.Next(Register.Match.FieldSize.Y + 1)

                Return New Coordinates(xCoord, yCoord)
            End Function

            Private Function RandomShipOrientation() As ShipOrientation
                Dim orientations = {ShipOrientation.Horizontal, ShipOrientation.Vertical}

                Return orientations(rand.Next(2))
            End Function

            Private Function NextRandomShot() As Shot
                Dim randomShotIndex = rand.Next(shotQueue.Count)

                Dim randomShot = shotQueue(randomShotIndex)

                shotQueue.Remove(randomShot)

                Return randomShot
            End Function

            Private Sub SetShots()
                shotQueue = New ShotList()

                shotQueue.MakeReceivers(Register.Opponents)

                shotQueue.Invert(Register.Match.FieldSize)
            End Sub

        End Class
    End Namespace
End Namespace