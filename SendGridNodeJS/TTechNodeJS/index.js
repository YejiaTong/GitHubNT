require('dotenv').config({ path: 'process.env' });
const http = require('http');

http.createServer(function (req, res) {
    // parses the request url
    /*var theUrl = url.parse( req.url );
	var queryObj = queryString.parse( theUrl.query );
	var emailObj = JSON.parse( queryObj.jsonData );
	var emailStr = '<p>Name:&nbsp;' + emailObj.name + '</p>' 
		+ '<p>Email:&nbsp;' + emailObj.email + '</p>'
		+ '<p>Phone:&nbsp;' + emailObj.phone + '</p>'
		+ '<p>Subject:&nbsp;' + emailObj.subject + '</p>'
		+ '<p>Message:&nbsp;' + emailObj.message + '</p>';*/
    var emailStr = '<p>' + req + '</p>';

    var date = new Date();
    console.log('[' + date + '] A new request arrived with HTTP headers: ' + JSON.stringify(req.headers));
    res.writeHead(200, { 'Content-Type': 'text/html' });
    res.end('<!DOCTYPE html><html><head><title>TTech Node JS</title></head><body><p style="font-size: xx-large;"><b>Hello, World! Node JS.</b></p><p style="font-size: larger;"><b>Visit &#64; ' + date + '</b></p>'
      + '<blockquote>&#45;&#45;&#45; Noah Y. Tong</blockquote><br />[helloworld sample; iisnode version is '
      + process.env.IISNODE_VERSION + ', node version is ' + process.version + ']</body></html>');
}).listen(process.env.PORT);

console.log('Application started at location ' + process.env.PORT);