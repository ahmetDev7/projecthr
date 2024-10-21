import unittest
from httpx import Client
from httpx import codes

# 6 happy paths
# 8 edge cases


class Testitem_groups(unittest.TestCase):

    def setUp(self):
        self.reader_token = "f6g7h8i9j0"
        self.client = Client(base_url="http://localhost:3000/api/v1/",
                             headers={"Content-Type": "application/json", "API_KEY": "a1b2c3d4e5"})

    def test_get_all_readertoken(self):
        self.client.headers["API_KEY"] = self.reader_token
        response = self.client.get("/item_groups")

        self.assertEqual(response.status_code, codes.OK)
    
    def test_get_single_readertoken(self):
        self.client.headers["API_KEY"] = self.reader_token
        response = self.client.get("/item_groups/8")

        self.assertEqual(response.status_code, codes.OK)
    
    def test_unauthorized_get_all(self):
        self.client.headers["API_KEY"] = "wrong_key"
        response = self.client.get("/item_groups")

        self.assertEqual(response.status_code, codes.UNAUTHORIZED)

    def test_get_single_incorrect_unauthorized(self):
        self.client.headers["API_KEY"] = "wrong_key"
        response = self.client.get("/item_groups/-90")

        self.assertEqual(response.status_code, codes.UNAUTHORIZED)

    def test_get_single_unauthorized(self):
        self.client.headers["API_KEY"] = "wrong_key"
        response = self.client.get("/item_groups/8")

        self.assertEqual(response.status_code, codes.UNAUTHORIZED)

    def test_update_unauthorized(self):
        self.client.headers["API_KEY"] = "wrong_key"
        updated_client = {
            "id": 3,
            "name": "Osman Smith",
            "description": "I did it mensen",
            "created_at": "1998-05-15 19:52:53",
            "updated_at": "2000-11-20 08:37:56"
        }

        response = self.client.put("/item_groups/3", json=updated_client)

        self.assertEqual(response.status_code, codes.UNAUTHORIZED)

    def test_delete_unauthorized(self):
        self.client.headers["API_KEY"] = "wrong_key"
        response = self.client.delete("/item_groups/90")

        self.assertEqual(response.status_code, codes.UNAUTHORIZED)

    def test_delete_incorrect_id_unauthorized(self):
        self.client.headers["API_KEY"] = "wrong_key"
        response = self.client.delete("/item_groups/-10")

        self.assertEqual(response.status_code, codes.UNAUTHORIZED)

    def test_get_all(self):
        response = self.client.get("/item_groups")

        self.assertEqual(response.status_code, codes.OK)
        self.assertTrue(len(response.json()) > 0)

    def test_get_single(self):
        response = self.client.get("/item_groups/8")

        self.assertEqual(response.status_code, codes.OK)
        self.assertIn("id", response.json())

    def test_get_incorrect_id(self):
        response = self.client.get("/item_groups/-1")

        self.assertEqual(response.status_code, codes.NOT_FOUND)

    def test_update_item_groups(self):
        updated_client = {
            "id": 0,
            "name": "Electronics",
            "description": "I did it mensen",
            "created_at": "1998-05-15 19:52:53",
            "updated_at": "2000-11-20 08:37:56"
        }

        response = self.client.put("/item_groups/0", json=updated_client)

        self.assertEqual(response.status_code, codes.OK)

    def test_delete_client(self):
        response = self.client.delete("/item_groups/90")

        self.assertEqual(response.status_code, codes.OK)

    def test_delete_incorrect_id(self):
        response = self.client.delete("/item_groups/-10")

        self.assertEqual(response.status_code, codes.NOT_FOUND)

if __name__ == "__main__":
    unittest.main()