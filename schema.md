{
  "player_progression": {
    "player_id": "uuid-string-format", // ADD THIS
    "current_xp": "integer",
    "current_tier": "string",
    "tier_progression": {
      "rookie": {
        "min_xp": "integer",
        "max_xp": "integer",
        "display_name": "string",
        "unlock_features": ["string"]
      }
    },
    "xp_history": [
      {
        "timestamp": "ISO-8601-string-format", // ADD THIS
        "xp_gained": "integer",
        "source": "string", // "match_played", "pack_pull", "facility_boost"
        "facility_multiplier": "float", // ADD THIS
        "coaching_bonus": "float" // ADD THIS
      }
    ]
  }
}























Are total weeks in a season always going to be 10? If so we would not need total_weeks field
Is rating same as total_xp?
Are points same as total_xp gained?
is current_level same as current_tier



{
  "season_id": "S_cae55b1e",
  "week": 0;
  "teams": [
    {
      "team_id": "T_254123",
      "player_id": "P_74261",
      "team_name": "Kansas City Chiefs",
      "rating": 920,
      "is_player_team": false,
      "rank": 0,
      "stats": {
        "wins": 0,
        "losses": 0,
        "points": 0,
        "total_matches": 0
      },
      "progression": {
        "player_id":"P_74261",
        "current_xp": 0,
        "current_tier": 1, 
        "xp_history": [],
        "tier_progression" : {}
      }
    },
    {
      "team_id": "T_728fd6",
      "team_name": "Buffalo Bills",
      "rating": 900,
      "is_player_team": false,
      "rank": 0,
      "stats": {
        "wins": 0,
        "losses": 0,
        "ties": 0,
        "points": 0,
        "total_matches": 0
      },
      "progression": {
        "total_xp": 0,
        "current_level": 1,
        "xp_history": [],
        "tier": "Rookie"
      }
    },
    {
      "team_id": "T_594596",
      "team_name": "Dallas Cowboys",
      "rating": 880,
      "is_player_team": true,
      "rank": 0,
      "stats": {
        "wins": 0,
        "losses": 0,
        "ties": 0,
        "points": 0,
        "total_matches": 0
      },
      "progression": {
        "total_xp": 0,
        "current_level": 1,
        "xp_history": [],
        "tier": "Rookie"
      }
    },
    {
      "team_id": "T_2300a6",
      "team_name": "San Francisco 49ers",
      "rating": 910,
      "is_player_team": false,
      "rank": 0,
      "stats": {
        "wins": 0,
        "losses": 0,
        "ties": 0,
        "points": 0,
        "total_matches": 0
      },
      "progression": {
        "total_xp": 0,
        "current_level": 1,
        "xp_history": [],
        "tier": "Rookie"
      }
    },
    {
      "team_id": "T_007676",
      "team_name": "Green Bay Packers",
      "rating": 850,
      "is_player_team": false,
      "rank": 0,
      "stats": {
        "wins": 0,
        "losses": 0,
        "ties": 0,
        "points": 0,
        "total_matches": 0
      },
      "progression": {
        "total_xp": 0,
        "current_level": 1,
        "xp_history": [],
        "tier": "Rookie"
      }
    },
    {
      "team_id": "T_9b33e9",
      "team_name": "Pittsburgh Steelers",
      "rating": 840,
      "is_player_team": false,
      "rank": 0,
      "stats": {
        "wins": 0,
        "losses": 0,
        "ties": 0,
        "points": 0,
        "total_matches": 0
      },
      "progression": {
        "total_xp": 0,
        "current_level": 1,
        "xp_history": [],
        "tier": "Rookie"
      }
    },
    {
      "team_id": "T_1c78f8",
      "team_name": "New England Patriots",
      "rating": 820,
      "is_player_team": false,
      "rank": 0,
      "stats": {
        "wins": 0,
        "losses": 0,
        "ties": 0,
        "points": 0,
        "total_matches": 0
      },
      "progression": {
        "total_xp": 0,
        "current_level": 1,
        "xp_history": [],
        "tier": "Rookie"
      }
    },
    {
      "team_id": "T_932a27",
      "team_name": "Philadelphia Eagles",
      "rating": 890,
      "is_player_team": false,
      "rank": 0,
      "stats": {
        "wins": 0,
        "losses": 0,
        "ties": 0,
        "points": 0,
        "total_matches": 0
      },
      "progression": {
        "total_xp": 0,
        "current_level": 1,
        "xp_history": [],
        "tier": "Rookie"
      }
    }
  ],
  "season_goals": {
    "playoffs": "Reach playoffs (Top 4)",
    "xp_target": "Earn 1000 XP this season"
  }
}