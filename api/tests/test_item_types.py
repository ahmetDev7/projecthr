import unittest
from httpx import Client
from httpx import codes

# 5 test integrations happy path

class TestItemTypes(unittest.TestCase):

    def setUp(self):
        self.client = Client(base_url="http://localhost:3000/api/v1/", headers={"Content-Type": "application/json", "API_KEY": "a1b2c3d4e5"})

    def test_get_all(self):
        response = self.client.get("/item_types")

        self.assertEqual(response.status_code, codes.OK)
        self.assertTrue(len(response.json()) > 0)
        
    def test_get_single(self):
        response = self.client.get("/item_types/1")

        self.assertEqual(response.status_code, codes.OK)
        self.assertIn("id", response.json())

    def test_get_all_items_specific_single(self):
        response = self.client.get("/item_types/1/items")

        self.assertEqual(response.status_code, codes.OK)

    def test_update_item_type(self):
        updated_item_type = {
            "name": "PC1",
            "description": "xd",
        }
        response = self.client.put("/item_types/1", json=updated_item_type)

        self.assertEqual(response.status_code, codes.OK) # HTTP 200 OK

    def test_delete_item_type(self):
        response = self.client.delete("/item_types/4")

        self.assertEqual(response.status_code, codes.OK)  # HTTP 200 OK

    
