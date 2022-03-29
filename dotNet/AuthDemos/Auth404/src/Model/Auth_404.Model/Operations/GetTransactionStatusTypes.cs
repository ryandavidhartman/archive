using System.Collections.Generic;
using Auth_404.Model.Data;
using RESTServiceUtilities.Interfaces;
using ServiceStack;

namespace Auth_404.Model.Operations
{
    [Api("Return a List of Transaction Status Type Resources")]
    [Route("/TransactionStatusTypes", "GET")]
    [Route("/TransactionStatusTypes/{Ids}", "GET")]
    public class GetTransactionStatusTypes : IReturn<List<TransactionStatusType>>, IDatas
    {
        public List<long> Ids { get; set; }
    }
}