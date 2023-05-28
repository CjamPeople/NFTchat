from flask import Flask, request, jsonify
from web3 import Web3
import json

app = Flask(__name__)
# Klaytn API URL
w3 = Web3(Web3.HTTPProvider('https://api.baobab.klaytn.net:8651'))

with open('abi.json') as f:
    abi_data = json.load(f)

abi = abi_data['abi']


@app.route('/mint', methods=['POST'])
def mint():
    data = request.get_json()
    private_key = data['private_key']
    account = w3.eth.account.privateKeyToAccount(private_key)

    contract = w3.eth.contract(
        '0x5f28482D46115a11566c04a18f42156B78c47434', abi=abi)

    nonce = w3.eth.getTransactionCount(account.address)
    tx = contract.functions.mintChatToken(data['tokenURI']).buildTransaction({
        'from': account.address,
        'gas': 200000,
        'nonce': nonce
    })

    signed_tx = account.sign_transaction(tx)
    tx_hash = w3.eth.send_raw_transaction(signed_tx.rawTransaction)

    # You might want to wait for the transaction to be mined here.

    receipt = w3.eth.wait_for_transaction_receipt(tx_hash)
    token_id = receipt['events']['Transfer']['tokenId']
    owner = contract.functions.ownerOf(token_id).call()

    return jsonify({
        'token_id': token_id,
        'tx_hash': tx_hash.hex(),
        'owner': owner
    }), 200


@app.route('/airdrop', methods=['POST'])
def airdrop():
    data = request.get_json()
    private_key = data['private_key']
    account = w3.eth.account.privateKeyToAccount(private_key)

    contract = w3.eth.contract(
        '0x5f28482D46115a11566c04a18f42156B78c47434', abi=abi)

    nonce = w3.eth.getTransactionCount(account.address)
    tx = contract.functions.transferFrom(data['to_address'], data['token_id']).buildTransaction({
        'from': account.address,
        'gas': 200000,
        'nonce': nonce
    })

    signed_tx = account.sign_transaction(tx)
    tx_hash = w3.eth.send_raw_transaction(signed_tx.rawTransaction)

    return jsonify({'tx_hash': tx_hash.hex()}), 200


if __name__ == '__main__':
    app.run(debug=True)
