using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using Models;

//TODO: We should really add more tests? Or should we remove those - nobody uses them anyway...
public class PlayerModelTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void JumpTakeOffSpeed_ByDefault_IsSeven()
    {
        // Arrange
        var playerModel = new PlayerModel();

        // Act
        
        // Assert
        Assert.AreEqual(GameConstants.PlayerJumpTakeOffSpeed, playerModel.jumpTakeOffSpeed);
    }
}
