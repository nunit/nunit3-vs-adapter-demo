# NUnit3 Test Demo #

The Test Demo is primarily intended to be opened in the IDE for the purpose of viewing the visual treatment of various elements. For that reason, there are failures, warnings and ignored tests as well as successful ones.

As a secondary purpose, the demos can be run in CI to ensure that the results under VSTest are as expected.

#### Structure ####

Each demo is represented by a VS solution containing a single project. This allows a single demo to be opened in the IDE. The project and solution are in the same directory in order to make scripting simpler and all demos are organized at the top level by the version of Visual Studio used to create them.

Source code is common for all demos and is organized by the language supported.

#### ExpectXxxx Attributes

The Test Demo code includes definition of special attributes like `ExpectPass` and `ExpectFailure` for use on the tests. This allows the tester to group the tests by Traits and easily see that the outcome for each group of expectations is what it should be.

#### C++ Demo ####

The C++ version of the Demo includes a much smaller number of tests since certain features of NUnit are not directly accessible from C++. More tests may be added as we develop shims for those features.