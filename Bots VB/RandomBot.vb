Imports MBC.Shared.Util
Imports MBC.Shared.Events
Imports MBC.Shared
Imports MBC.Shared.Attributes
Imports MBC.Shared.Entities

Namespace MBC
    Namespace Controllers
        ''' <summary>
        ''' An MBC battleship contender. There is no strategy behind the RandomBot besides randomly placing ships and randomly placing shots.
        ''' </summary>
        <Name("RandomBot VB")>
        <Version(2, 0)>
        Public Class RandomBot
            Inherits Controller2

            Private rand As Random
            Private shotsRemain As List(Of Shot)

            ''' <summary>
            ''' Creates the random generator and hooks required events.
            ''' </summary>
            Protected Overrides Sub Initialize()
                rand = New Random()
                AddHandler Player.Match.OnEvent, AddressOf PlaceShips
                AddHandler Player.Match.OnEvent, AddressOf CreateShots
                AddHandler Player.OnEvent, AddressOf Shoot
            End Sub

            ''' <summary>
            ''' In the beginning of a round, place all of the ships assigned to the player we're controlling
            ''' (they will all be reset).
            ''' </summary>
            <EventFilter(GetType(RoundBeginEvent))>
            Sub PlaceShips(ev As [Event])
                While Not ShipList.AreShipsPlaced(Player.Ships)
                    ShipList.PlaceShip(Player.Ships, RandomCoordinates(), RandomOrientation())
                End While
            End Sub

            ''' <summary>
            ''' At the start of a round (RoundBeginEvent), populate the list of shots to shoot at.
            ''' </summary>
            <EventFilter(GetType(RoundBeginEvent))>
            Sub CreateShots(ev As [Event])
                shotsRemain = New List(Of Shot)
                For Each opponent As Player In Player.Match.Players
                    If (opponent = Player) Then
                        Continue For
                    End If
                    QueueShotsPlayer(opponent)
                Next
            End Sub

            ''' <summary>
            ''' This method generates a random set of coordinates within the match field boundaries.
            ''' </summary>
            ''' <returns>Coordinates of randomized X and Y components within the field boundaries.</returns>
            Function RandomCoordinates() As Coordinates
                Dim x As Integer = rand.Next(Player.Match.FieldSize.X)
                Dim y As Integer = rand.Next(Player.Match.FieldSize.Y)
                Return New Coordinates(x, y)
            End Function

            ''' <summary>
            ''' This method randomly returns one of two ShipOrientation enums.
            ''' </summary>
            ''' <returns>A randomly selected ShipOrientation.</returns>
            Function RandomOrientation() As ShipOrientation
                Dim orientations() As ShipOrientation = New ShipOrientation(1) {ShipOrientation.Horizontal, ShipOrientation.Vertical}
                Return orientations(rand.Next(2))
            End Function

            ''' <summary>
            ''' Populates the shotRemain list with all possible shots against a Player opponent
            ''' </summary>
            Sub QueueShotsPlayer(opponent As Player)
                For x As Integer = 0 To Player.Match.FieldSize.X - 1
                    For y As Integer = 0 To Player.Match.FieldSize.Y - 1
                        shotsRemain.Add(New Shot(opponent, New Coordinates(x, y)))
                    Next
                Next
            End Sub

            ''' <summary>
            ''' At the beginning of our Player's turn (PlayerTurnBeginEvent), shoot at a random coordinate
            ''' on our opponent's battlefield.
            ''' </summary>
            <EventFilter(GetType(PlayerTurnBeginEvent))>
            Sub Shoot(ev As [Event])
                Dim shotIdx As Integer = rand.Next(shotsRemain.Count)
                Dim shot As Shot = shotsRemain(shotIdx)
                shotsRemain.RemoveAt(shotIdx)
                Player.Shoot(shot)
            End Sub

        End Class
    End Namespace
End Namespace