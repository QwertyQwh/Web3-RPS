using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts;
using System.Threading;
using RPSContract.Contracts.RPS.ContractDefinition;

namespace RPSContract.Contracts.RPS
{
    public partial class RPSService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, RPSDeployment rPSDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<RPSDeployment>().SendRequestAndWaitForReceiptAsync(rPSDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, RPSDeployment rPSDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<RPSDeployment>().SendRequestAsync(rPSDeployment);
        }

        public static async Task<RPSService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, RPSDeployment rPSDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, rPSDeployment, cancellationTokenSource);
            return new RPSService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public RPSService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<bool> GetMatchingStatusQueryAsync(GetMatchingStatusFunction getMatchingStatusFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetMatchingStatusFunction, bool>(getMatchingStatusFunction, blockParameter);
        }

        
        public Task<bool> GetMatchingStatusQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetMatchingStatusFunction, bool>(null, blockParameter);
        }

        public Task<string> PayForEntranceRequestAsync(PayForEntranceFunction payForEntranceFunction)
        {
             return ContractHandler.SendRequestAsync(payForEntranceFunction);
        }

        public Task<string> PayForEntranceRequestAsync()
        {
             return ContractHandler.SendRequestAsync<PayForEntranceFunction>();
        }

        public Task<TransactionReceipt> PayForEntranceRequestAndWaitForReceiptAsync(PayForEntranceFunction payForEntranceFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(payForEntranceFunction, cancellationToken);
        }

        public Task<TransactionReceipt> PayForEntranceRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<PayForEntranceFunction>(null, cancellationToken);
        }

        public Task<string> SendChoiceRequestAsync(SendChoiceFunction sendChoiceFunction)
        {
             return ContractHandler.SendRequestAsync(sendChoiceFunction);
        }

        public Task<TransactionReceipt> SendChoiceRequestAndWaitForReceiptAsync(SendChoiceFunction sendChoiceFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(sendChoiceFunction, cancellationToken);
        }

        public Task<string> SendChoiceRequestAsync(BigInteger choice)
        {
            var sendChoiceFunction = new SendChoiceFunction();
                sendChoiceFunction.Choice = choice;
            
             return ContractHandler.SendRequestAsync(sendChoiceFunction);
        }

        public Task<TransactionReceipt> SendChoiceRequestAndWaitForReceiptAsync(BigInteger choice, CancellationTokenSource cancellationToken = null)
        {
            var sendChoiceFunction = new SendChoiceFunction();
                sendChoiceFunction.Choice = choice;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(sendChoiceFunction, cancellationToken);
        }

        public Task<string> StartMatchingRequestAsync(StartMatchingFunction startMatchingFunction)
        {
             return ContractHandler.SendRequestAsync(startMatchingFunction);
        }

        public Task<string> StartMatchingRequestAsync()
        {
             return ContractHandler.SendRequestAsync<StartMatchingFunction>();
        }

        public Task<TransactionReceipt> StartMatchingRequestAndWaitForReceiptAsync(StartMatchingFunction startMatchingFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(startMatchingFunction, cancellationToken);
        }

        public Task<TransactionReceipt> StartMatchingRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<StartMatchingFunction>(null, cancellationToken);
        }

        public Task<string> WithDrawRequestAsync(WithDrawFunction withDrawFunction)
        {
             return ContractHandler.SendRequestAsync(withDrawFunction);
        }

        public Task<string> WithDrawRequestAsync()
        {
             return ContractHandler.SendRequestAsync<WithDrawFunction>();
        }

        public Task<TransactionReceipt> WithDrawRequestAndWaitForReceiptAsync(WithDrawFunction withDrawFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(withDrawFunction, cancellationToken);
        }

        public Task<TransactionReceipt> WithDrawRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<WithDrawFunction>(null, cancellationToken);
        }
    }
}
