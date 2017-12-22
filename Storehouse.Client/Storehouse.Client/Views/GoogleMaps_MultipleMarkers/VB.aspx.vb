Imports System.Data
Imports System.Configuration
Imports System.Data.SqlClient
Partial Class VB
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            Dim dt As DataTable = Me.GetData("select * from Locations")
            rptMarkers.DataSource = dt
            rptMarkers.DataBind()
        End If
    End Sub

    Private Function GetData(query As String) As DataTable
        Dim conString As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Dim cmd As New SqlCommand(query)
        Using con As New SqlConnection(conString)
            Using sda As New SqlDataAdapter()
                cmd.Connection = con

                sda.SelectCommand = cmd
                Using dt As New DataTable()
                    sda.Fill(dt)
                    Return dt
                End Using
            End Using
        End Using
    End Function
End Class
