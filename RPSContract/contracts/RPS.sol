// SPDX-License-Identifier: MIT
pragma solidity >=0.4.22 <0.9.0;

error AlreadyMatchingError();
error NotEnoughEntranceFeeError();
error EntranceFeeNotPaidError();
error NoMatchError();
error ChoiceConflictError();
error ChoiceOutOfRangeError();


contract RPS{
    mapping(address => address) matching;
    address waiting;
    mapping(address=> uint) deposits;
    mapping(address=>uint) rewards;
    mapping(address=> uint) choices;
    event Cleared(address indexed _player, int win);
    uint constant Ticket = 1000000000000000000;


    function PayForEntrance() public payable{
        if(msg.value < Ticket){
            revert NotEnoughEntranceFeeError();
        }
        deposits[msg.sender] += msg.value;
    }


    function StartMatching() public {
        if(deposits[msg.sender]<Ticket){
            revert EntranceFeeNotPaidError();
        }
        if(waiting == msg.sender || matching[msg.sender] != address(0)){
            revert AlreadyMatchingError();
        }
        if(waiting != address(0)){
            matching[msg.sender] = waiting;
            matching[waiting] = msg.sender;
        }else{
            waiting = msg.sender;
        }
    }
    
    function GetMatchingStatus() public view returns(bool){
        if(matching[msg.sender] != address(0)){
            return true;
        }
        return false;
    }

    function SendChoice(uint choice) public {
        // Hasn't matched
        if(!GetMatchingStatus()){
            revert NoMatchError();
        }
        //Choice already sent
        if(choices[msg.sender] != 0){
            revert ChoiceConflictError();
        }
        if(choice == 0 || choice >3){
            revert ChoiceOutOfRangeError();
        }
        address opponent = matching[msg.sender];
        uint otherChoice = choices[opponent];
        // The opponent hasn't made their decision, (but should be as soon as their GetMatchingStatus returns true)
        if(otherChoice == 0){
            choices[msg.sender] = choice;
            return;
        }
        // Both parties have made decisions. It's time for a clearance. 
        int win = 0;
        if(choice != otherChoice){
            if(choice == 3){
                if(otherChoice == 1){
                    win = -1;
                }else{
                    win = 1;
                }
            }else if(choice == 1){
                if(otherChoice == 2){
                    win = -1;
                }else{
                    win = 1;
                }
            }else{
                if(otherChoice == 3){
                    win = -1;
                }else{
                    win = 1;
                }
            }
        }
        if(win >=0 )
            rewards[msg.sender] += deposits[msg.sender];
        else
            rewards[opponent] += deposits[msg.sender];
        if(win <=0)
            rewards[opponent] += deposits[opponent];
        else
            rewards[msg.sender] += deposits[opponent];
        
        // Clear the deposits
        deposits[msg.sender] =0;
        deposits[opponent] = 0;
        // Notify the waiting parties
        choices[msg.sender] = 0;
        choices[opponent] = 0;
        //Clear everything up 
        delete matching[opponent]; 
        delete matching[msg.sender];
        waiting = address(0);
        emit Cleared(msg.sender, win);
        emit Cleared(opponent, -win);
    }


    function WithDraw() public{
        uint amount = rewards[msg.sender];
        payable(msg.sender).transfer(amount);
        rewards[msg.sender] = 0;
    }

}