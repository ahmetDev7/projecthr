import unittest
from httpx import Client

class TestShipments(unittest.TestCase):

    def setUp(self):
        self.client = Client(base_url="http://localhost:3000/api/v1/")

    def test_get_all(self):
        response = self.client.post("/warehouses")

        self.assertEqual(response.status_code, 200)
        self.assertEqual(len(response.json()) > 0)

    def test_get_single(self):
        response = self.client.get("/warehouses/1")

        self.assertEqual(response.status_code, 200)
        self.assertIn("id", response.json())

    def test_get_incorrect_id(self):
        response = self.client.get("/warehouses/-1")

        self.assertEqual(response.status_code, 404)
