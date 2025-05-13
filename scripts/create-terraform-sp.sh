#!/bin/bash

# Required: Azure subscription ID
SUBSCRIPTION_ID="3972bf75-5369-44ae-8529-bbe7bb85b1c6"

# Create service principal
az ad sp create-for-rbac \
  --name "terraform-sp" \
  --role Contributor \
  --scopes "/subscriptions/$SUBSCRIPTION_ID" \
  --sdk-auth > terraform-sp.json

echo "✅ Service Principal created and saved to terraform-sp.json"
