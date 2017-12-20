Imports NUnit.Framework

Namespace NUnitTestDemo

    Public Class AsyncTests

        <Test, ExpectError>
        Public Async Sub AsyncVoidTestIsInvalid()
            Dim result = Await ReturnOne()

            Assert.AreEqual(1, result)
        End Sub

        <Test, ExpectPass>
        Public Async Function AsyncTaskTestSucceeds() As Task
            Dim result = Await ReturnOne()

            Assert.AreEqual(1, result)
        End Function

        <Test, ExpectFailure>
        Public Async Function AsyncTaskTestFails() As Task
            Dim result = Await ReturnOne()

            Assert.AreEqual(2, result)
        End Function

        <Test, ExpectError>
        Public Async Function AsyncTaskTestThrowsException() As Task
            Await ThrowException()

            Assert.Fail("Should never get here")
        End Function

        <TestCase(ExpectedResult:=1), ExpectPass>
        Public Async Function AsyncTaskWithResultSucceeds() As Task(Of Integer)
            Return Await ReturnOne()
        End Function

        <TestCase(ExpectedResult:=2), ExpectFailure>
        Public Async Function AsyncTaskWithResultFails() As Task(Of Integer)
            Return Await ReturnOne()
        End Function

        <TestCase(ExpectedResult:=0), ExpectError>
        Public Async Function AsyncTaskWithResultThrowsException() As Task(Of Integer)
            Return Await ThrowException()
        End Function

        Private Shared Function ReturnOne() As Task(Of Integer)
            Return Task.Run(
                Function()
                    Return 1
                End Function
            )
        End Function

        Private Shared Function ThrowException() As Task(Of Integer)
            Return Task.Run(
                Function()
                    Throw New InvalidOperationException()
                    Return 1
                End Function
            )
        End Function

    End Class

End Namespace