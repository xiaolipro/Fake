/**
* JetBrains Space Automation
* This Kotlin script file lets you automate build activities
* For more info, see https://www.jetbrains.com/help/space/automation.html
*/

job("Hello World!") {
    container(displayName = "Say Hello", image = "hello-world")
}

job("Qodana") {
    container("jetbrains/qodana-dotnet") {
        shellScript {
            content = """
               qodana \
               --fail-threshold <number> \
               --profile-name <profile-name>
               """.trimIndent()
        }
    }
}