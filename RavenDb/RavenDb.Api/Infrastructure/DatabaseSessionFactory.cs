using System;
using Microsoft.CodeAnalysis;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Session;
using RabenDb.Api.Constants;

namespace RabenDb.Api.Infrastructure {
    public class DatabaseSessionFactory {

        private static Lazy<IDocumentStore> _lazyDocStore = new Lazy<IDocumentStore> (() => InitializeSessionFactory ());

        private DatabaseSessionFactory () { }

        private static IDocumentStore SessionFactory {
            get {
                return _lazyDocStore.Value;
            }
        }

        public static IDocumentSession OpenSession () {
            return SessionFactory.OpenSession (DbConstants.DB_NAME);
        }

        private static IDocumentStore InitializeSessionFactory () {
            var _docStore = new DocumentStore { Urls = DbConstants.DB_URL };
            _docStore.Initialize ();
            IndexCreation.CreateIndexes (typeof (Location).Assembly, _docStore);

            return _docStore;
        }
    }
}