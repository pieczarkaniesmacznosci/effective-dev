const express = require('express');
const path = require('path');
const PROTO_PATH = path.join(__dirname, 'articles.proto');
const protoLoader = require('@grpc/proto-loader');
const grpc = require('@grpc/grpc-js');

const packageDefinition = protoLoader.loadSync(
  PROTO_PATH,
  {
    keepCase: true,
   longs: String,
   enums: String,
   defaults: true,
   oneofs: true
  }
);

const article_proto = grpc.loadPackageDefinition(packageDefinition).article;
var client = new article_proto.ArticlesBasicService('localhost:5000', grpc.credentials.createInsecure());

const app = express();

app.use(express.json());
app.use(express.urlencoded({ extended: false }));

app.get('/', (req, res) => {
  client.CreateArticle({summary: "Neko"}, function(err, response) {
    console.log('Response:', response.id);
    res.send(response.id)
  });

});

app.listen(3000);