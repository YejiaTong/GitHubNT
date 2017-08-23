// using SendGrid's v3 Node.js Library
// https://github.com/sendgrid/sendgrid-nodejs
// Set up environment variables
require('dotenv').config({path: 'process.env'});
const express = require('express');
const bodyParser = require('body-parser');
const cryptoJS = require('crypto-js');
/* Disable querystring - Noah
 * const querystring = require('querystring');
*/
const app = express();

// Set up JSON body parser
app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());
// Wildcard catch
app.all('/sendgrid/*', function (req, res) {
	var date = new Date();
	// log
	var url = req.get('host');
	var hd = JSON.stringify(req.headers);
	console.log('[' + date + '] Email request arrived with URL: ' + url 
		+ ' HTTP headers: ' + hd);
	
	// parses the request url
	var sendGridKey = process.env.SENDGRID_API_KEY;
	var sendGridAccessKey = process.env.SENDGRID_API_ACCESS_KEY;
	var sendGridCryptoKey = process.env.SENDGRID_API_CRYPTO_KEY;
	var sendGridAutoEmail = process.env.SENDGRID_API_AUTO_EMAIL;
	var success = true;
	var error;
	var emailFrom;
	
	try {
		if (req.body.key === undefined) {
			throw 'Insufficient access rights';
		}
		if (req.body.email === undefined) {
			throw 'Insufficient access rights';
		}
		
		// encryption validation
		var cryptoRet = cryptoJS.HmacSHA1(url + req.body.key, sendGridAccessKey);
		console.log(cryptoRet);
		if (cryptoRet != sendGridCryptoKey) {
			throw 'HACKING PROHIBITED! - ' + hd;
		}
		
		console.log('[' + date + '] Request detail { "Name" : ' + req.body.name 
			+ ' , "Email" : ' + req.body.email + ' , "Phone" : ' + req.body.phone
			+ ' , "Subject" : ' + req.body.subject + ' , "Message" : ' + req.body.message);
		
		// Send email to auto email server
		var helper = require('sendgrid').mail;
		var fromEmail = new helper.Email(req.body.email);
		emailFrom = fromEmail;
		var toEmail = new helper.Email(sendGridAutoEmail);
		var subject = req.body.subject + ' ' + req.body.name;
		var content = new helper.Content('text/plain', 'From: ' + req.body.name 
			+ ', ' + req.body.phone + ' Message: ' + req.body.message);
		var mail = new helper.Mail(fromEmail, subject, toEmail, content);

		var sg = require('sendgrid')(sendGridKey);
		var request = sg.emptyRequest({
			method: 'POST',
			path: '/v3/mail/send',
			body: mail.toJSON()
		});
		
		sg.API(request, function (error, response) {
			if (error) {
				console.log('Error response received');
			}
			console.log(response.statusCode);
			console.log(response.body);
			console.log(response.headers);
		});
		
		// Send acknowledgement email back to client
		// Placeholder - to be completed
	}
	catch (err) {
		console.log('[' + date + '] Error occurred: ' + err);
		success = false;
		error = err;
	}
	finally {
		if (success) {
			res.header('Access-Control-Allow-Origin', '*');
			res.header('Access-Control-Allow-Methods', 'POST');
			//GET,PUT,POST,DELETE
			res.header('Access-Control-Allow-Headers', 'X-Requested-With');
			console.log('[' + date + '] Email sent successfully. FromMail: ' + emailFrom);
			res.statusCode = 200;
			res.send({ 'success': 'true' });
		} else {
			res.header('Access-Control-Allow-Origin', '*');
			res.header('Access-Control-Allow-Methods', 'POST');
			//GET,PUT,POST,DELETE
			res.header('Access-Control-Allow-Headers', 'X-Requested-With');
			res.statusCode = 200;
			res.send({ 'success' : 'false',
				'msg' : 'Failed to send email. Error: ' + error});
		}
	}
});

app.listen(process.env.PORT);