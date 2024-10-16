import unittest
from httpx import Client
from httpx import codes

# 5 happy paths
# 1 edge case
class TestShipments(unittest.TestCase):

    def setUp(self):
        self.client = Client(base_url="http://localhost:3000/api/v1/",
                             headers={"Content-Type": "application/json", "API_KEY": "a1b2c3d4e5"})

    def test_get_all(self):
        response = self.client.get("/clients")

        self.assertEqual(response.status_code, 200)
        self.assertTrue(len(response.json()) > 0)

    def test_get_single(self):
        response = self.client.get("/clients/8")

        self.assertEqual(response.status_code, codes.OK)
        self.assertIn("id", response.json())

    def test_get_incorrect_id(self):
        response = self.client.get("/clients/-1")

        self.assertEqual(response.status_code, codes.OK)  # HTTP 200 OK

    def test_create_client(self):
        new_shipment = {
            "id": 3,
            "name": "Johan van der Berg",
            "email":"123",
            "phone":"123",
            "address":"123",
            "city":"123",
            "postal_code":"123",
            "country":"123",
            "shipment_id": 1,
            "created_at": "1973-02-24 07:36:32",

        }

        response = self.client.post("/clients", json=new_shipment)
        self.assertEqual(response.status_code, 201)  # HTTP 201 Created

    def test_update_client(self):
        updated_client = {
            "id": 3,
            "name": "Park",
            "address": "Osman straat 7",
        }

        response = self.client.put("/clients/3", json=updated_client)

        self.assertEqual(response.status_code, codes.OK)  # HTTP 200 OK

    def test_delete_client(self):
        response = self.client.delete("/clients/7")

        self.assertEqual(response.status_code, 200)  # HTTP 204 No Content

        # Test if item still exists after deletion
        # checkDeletionResponse = self.client.get("/shipments/7")
        # self.assertEqual(checkDeletionResponse, 404)  # HTTP 404 (Because it no longer exists)
