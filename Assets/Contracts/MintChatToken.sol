// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

import "@klaytn/contracts/KIP/token/KIP17/KIP17.sol";

contract MintChatToken is KIP17 {
    
    struct chatData{
        string tokenURI;
        address user;
    }

    chatData[] private chatDatas;

    constructor() KIP17("Plain", "LOBOS") {}

    function mintChatToken(string memory _tokenURI) public {
        uint256 chatTokenId = chatDatas.length;
        chatDatas.push(chatData(_tokenURI, msg.sender));
        _mint(msg.sender, chatTokenId);
    }

    function getChatMember(uint256 chatTokenId) public view returns (address) {
        return ownerOf(chatTokenId);
    }
}

contract UnityInterface {
    MintChatToken private chatTokenContract;

    constructor(address _chatTokenContract) {
        chatTokenContract = MintChatToken(_chatTokenContract);
    }

    function mintChatToken(string memory _tokenURI) public {
        chatTokenContract.mintChatToken(_tokenURI);
    }
    
}
