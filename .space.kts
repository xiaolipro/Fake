/**
* JetBrains Space Automation
* This Kotlin script file lets you automate build activities
* For more info, see https://www.jetbrains.com/help/space/automation.html
*/

job("Hello World!") {
    container(displayName = "Say Hello", image = "hello-world")
}

job("Qodana") {
   startOn {
      gitPush {
         anyBranchMatching  {
            // add 'master'
            +"master"
            // add all branches containing 'feature'
            +"*feature*"
            // exclude 'test-feature'
            -"test-feature"
         }
         // run only if there's a release tag
         // e.g., release/v1.0.0
         anyTagMatching {
             +"release/*"
             // but exclude beta releases
             -"release/*-beta"
         }
      }
      codeReviewOpened{}
   }
   container("jetbrains/qodana-dotnet") {
      env["QODANA_TOKEN"] = Secrets("qodana-token")
      shellScript {
          content = """qodana"""
      }
   }
}